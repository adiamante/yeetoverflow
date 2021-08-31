using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Reflection;

namespace YeetOverFlow.Core.Application.Tests
{
    public static class TestHelper
    {
        public static Mock<TRepository> GetMockYeetRepository<TRepository, TEntity>(params IEnumerable[] sourceLists) where TRepository : class, IRepository<TEntity> where TEntity : Entity
        {
            Mock<TRepository> mockRepository = new Mock<TRepository>();

            mockRepository.Setup(entity => entity.Delete(It.IsAny<TEntity>())).Callback<TEntity>(entity =>
            {
                foreach (IEnumerable list in sourceLists)
                {
                    ReflectionHelper.MethodInfoCollection[list.GetType()]["Remove"].Invoke(list, new object[] { entity });
                }
            });
            mockRepository.Setup(entity => entity.Delete(It.IsAny<object>())).Callback<object>(id =>
            {
                Guid guid = (Guid)id;
                foreach (IEnumerable<TEntity> list in sourceLists)
                {
                    TEntity entitytToRemove = list.FirstOrDefault(entity => entity.Guid == guid);
                    ReflectionHelper.MethodInfoCollection[list.GetType()]["Remove"].Invoke(list, new object[] { entitytToRemove });
                }
            });
            mockRepository.Setup(entity => entity.Get(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>(), It.IsAny<Expression<Func<TEntity, object>>>()))
                                                 .Returns<Expression<Func<TEntity, bool>>, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>, Expression<Func<TEntity, object>>>(
                                                     (filter, orderBy, includePropertyExpression) =>
                                                     {
                                                         IQueryable<TEntity> query = ((IEnumerable<TEntity>)sourceLists[0]).AsQueryable();
                                                         if (filter != null)
                                                         {
                                                             query = query.Where(filter);
                                                         }

                                                         if (orderBy != null)
                                                         {
                                                             return orderBy(query).ToList();
                                                         }
                                                         else
                                                         {
                                                             return query.ToList();
                                                         }
                                                     });
            mockRepository.Setup(entity => entity.GetById(It.IsAny<object>(), It.IsAny<Expression<Func<TEntity, object>>>()))
                                                 .Returns<object, Expression<Func<TEntity, object>>>(
                                                     (id, includePropertyExpression) =>
                                                     {
                                                         Guid guid = (Guid)id;
                                                         TEntity entity = ((IEnumerable<TEntity>)sourceLists[0]).FirstOrDefault(entity => entity.Guid == guid);
                                                         return entity;
                                                     });
            mockRepository.Setup(entity => entity.Delete(It.IsAny<object>())).Callback<object>(id =>
            {
                Guid guid = (Guid)id;

                foreach (IEnumerable<TEntity> list in sourceLists)
                {
                    TEntity entitytToRemove = list.FirstOrDefault(entity => entity.Guid == guid);
                    ReflectionHelper.MethodInfoCollection[list.GetType()]["Remove"].Invoke(list, new object[] { entitytToRemove });
                }
            });
            mockRepository.Setup(entity => entity.Insert(It.IsAny<TEntity>())).Callback<TEntity>(entity =>
            {
                foreach (IEnumerable list in sourceLists)
                {
                    //ReflectionHelper.MethodInfoCollection[list.GetType()]["Add"].Invoke(list, new object[] { entity });
                    RecursiveAdd<TEntity>(list, entity);
                }
            });
            mockRepository.Setup(entity => entity.Update(It.IsAny<TEntity>())).Callback<TEntity>(entity =>
            {
                foreach (IEnumerable<TEntity> list in sourceLists)
                {
                    TEntity entityMatch = list.FirstOrDefault(itm => itm.Guid == entity.Guid);
                    ReflectionHelper.MethodInfoCollection[list.GetType()]["Remove"].Invoke(list, new object[] { entityMatch });
                    ReflectionHelper.MethodInfoCollection[list.GetType()]["Add"].Invoke(list, new object[] { entity });
                }
            });

            return mockRepository;
        }

        private static void RecursiveAdd<TEntity>(IEnumerable collection, object itemToAdd)
        {
            MethodInfo addMethod = ReflectionHelper.MethodInfoCollection[collection.GetType()]["Add"];
            if (addMethod != null)
            {
                ReflectionHelper.MethodInfoCollection[collection.GetType()]["Add"].Invoke(collection, new object[] { itemToAdd });

                PropertyInfo childrenProperty = ReflectionHelper.PropertyInfoCollection[itemToAdd.GetType()]["Children"];
                if (childrenProperty != null)
                {
                    IEnumerable<TEntity> children = (IEnumerable<TEntity>)childrenProperty.GetValue(itemToAdd);
                    foreach (TEntity child in children)
                    {
                        RecursiveAdd<TEntity>(collection, child);
                    }
                }
            }
        }

        public static string TestOutput(YeetList yeetList)
        {
            //Get path for every node along with the name
            Dictionary<string, string> dictNodes = new Dictionary<string, string>();
            Dictionary<string, string> dictTerminalNodes = new Dictionary<string, string>();
            Dictionary<int, List<String>> dictLevels = new Dictionary<int, List<String>>();
            List<String> accumulator = new List<string>();
            dictLevels.Add(0, new List<string>());
            dictLevels.Add(1, new List<string>());
            dictLevels.Add(2, new List<string>());
            dictLevels.Add(3, new List<string>());
            dictLevels.Add(4, new List<string>());
            dictLevels.Add(5, new List<string>());

            //Resolve(YeetList, "0", 0, dictNodes, dictTerminalNodes, dictLevels);
            Resolve(yeetList, yeetList.Sequence.ToString(), 0, dictNodes, dictTerminalNodes, dictLevels);

            int totalWidth = 0;
            StringBuilder sb = new StringBuilder();

            int maxLevel = dictNodes.Keys.Max(k => k.Length) / 2;

            foreach (KeyValuePair<int, List<String>> kvpLevels in dictLevels)
            {
                int level = kvpLevels.Key;

                sb.Append("//");
                foreach (String path in kvpLevels.Value)
                {
                    if (String.IsNullOrWhiteSpace(path))
                    {
                        sb.Append(path);
                    }
                    else
                    {
                        String name = dictNodes[path];
                        bool hasSubKeys = dictTerminalNodes.Keys.Any(k => k != path && k.StartsWith(path + "."));

                        if (hasSubKeys)
                        {
                            int len = dictTerminalNodes.Where(kvp => kvp.Key != path && kvp.Key.StartsWith(path + ".")).Sum(k => k.Value.Length + 2);
                            int nameLen = name.Length;
                            int start = len / 2 - nameLen / 2 - (nameLen % 2 == 0 ? 1 : 2);
                            string display = "[" +                                //1 (left square bracket)
                                new String(' ', start) +                        //1 to halfway - half of name
                                $"{name}" +                                     //halfway
                                new String(' ', len - start - nameLen - 2) +    //halfway + half of name - 2 (for the square brackets)
                                "]";

                            if (level == 0)
                            {
                                totalWidth = len;
                                sb.Append(new String('*', totalWidth) + Environment.NewLine + "//");
                            }

                            sb.Append(display);
                        }
                        else
                        {
                            sb.Append($"[{name}]");
                        }
                    }
                }

                sb.AppendLine();

                if (level == maxLevel)
                {
                    break;
                }
            }

            sb.Append("//" + new String('*', totalWidth));
            TestContext.Out.WriteLine(sb.ToString());
            return sb.ToString();
        }

        private static void Resolve(YeetItem yeetItem,
                             String currentPath,
                             Int32 currentLevel,
                             Dictionary<string, string> dictNodes,
                             Dictionary<string, string> dictTerminalNodes,
                             Dictionary<int, List<string>> dictLevels)
        {
            dictLevels[currentLevel].Add(currentPath);

            dictNodes.Add(currentPath, yeetItem.Name);

            if (!(yeetItem is YeetList))
            {
                dictTerminalNodes.Add(currentPath, yeetItem.Name);
                if (dictLevels.Keys.Any(k => k > currentLevel))
                {
                    IEnumerable<int> higherLevels = dictLevels.Keys.Where(k => k > currentLevel);
                    foreach (int higherLevel in higherLevels)
                    {
                        dictLevels[higherLevel].Add(new string(' ', yeetItem.Name.Length + 2));
                    }
                }
            }
            switch (yeetItem)
            {
                case YeetList list:
                    //for (int i = 0; i < list.Children.Count; i++)
                    //{
                    //    Resolve(list.Children[i], currentPath + "." + i.ToString(), currentLevel + 1, dictNodes, dictTerminalNodes, dictLevels);
                    //}
                    List<YeetItem> childrenOrdered = list.Children.OrderBy(c => c.Sequence).ToList();
                    foreach (YeetItem child in childrenOrdered)
                    {
                        Resolve(child, currentPath + "." + child.Sequence.ToString(), currentLevel + 1, dictNodes, dictTerminalNodes, dictLevels);
                    }
                    break;
            }
        }
    }
}
