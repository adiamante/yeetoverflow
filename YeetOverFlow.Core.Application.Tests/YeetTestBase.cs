using System;
using System.Collections.Generic;
using NUnit.Framework;
using YeetOverFlow.Core;

namespace YeetOverFlow.Core.Application.Tests
{
    public class YeetTestBase
    {
        #region Shared variables
        protected YeetLibrary<YeetList> _library;
        protected YeetList _root, _list1, _list2, _list3, _newList;
        protected YeetItem _newItem, _itemA, _itemB, _itemC, _itemE, _itemF, _itemG, _itemI, _itemJ, _itemK, _itemX, _itemY, _itemZ;
        protected List<YeetLibrary<YeetList>> _libraries;
        protected List<YeetItem> _items;
        protected List<YeetList> _lists;
        protected List<IYeetEvent<YeetItem>> _events;
        #endregion Shared variables

        #region Setup
        [SetUp]
        public virtual void Setup()
        {
            InitVariables();
            InitAggregates();
        }
        #endregion Setup

        protected virtual void InitVariables()
        {
            _library = new YeetLibrary<YeetList>(Guid.Parse("546f8eb2-b4fd-4bbc-9e0f-522f931f557b"))
            {
                Owner = "Test_Owner"
            };

            _root = new YeetList(Guid.Parse("ef8b96cf-425c-4665-869c-53d121c0bb46"))
            {
                Name = "ROOT"
            };

            _list1 = new YeetList(Guid.Parse("19af6f2c-6ee4-4637-f58e-08d8b2a7b8a5"))
            {
                Name = "L1"
            };

            _list2 = new YeetList(Guid.Parse("db1b4480-2680-4b7a-cb34-08d8b59b3bee"))
            {
                Name = "L2"
            };

            _list3 = new YeetList(Guid.Parse("3236d972-61ad-4a2c-8b12-c773d1ae46e5"))
            {
                Name = "L3"
            };

            _newList = new YeetList(Guid.Parse("8eb6c0be-cca8-403c-af0e-1e56e3fa5743"))
            {
                Name = "L*"
            };

            _newItem = new YeetItem(Guid.Parse("5fff743a-911a-40e5-8dc2-0e9693bf6d20"))
            {
                Name = "I*"
            };

            _itemA = new YeetItem(Guid.Parse("5cbb0bcb-9c81-4ec3-a260-08d8b4f2b7f6"))
            {
                Name = "IA"
            };

            _itemB = new YeetItem(Guid.Parse("701139a8-f20b-4fa6-b52f-178e12362163"))
            {
                Name = "IB"
            };

            _itemC = new YeetItem(Guid.Parse("47e14d9a-51c1-4b7f-9f6d-dd3466c458a5"))
            {
                Name = "IC"
            };

            _itemE = new YeetItem(Guid.Parse("fd41384f-4da8-4cd8-b620-0df407fa6495"))
            {
                Name = "IE"
            };

            _itemF = new YeetItem(Guid.Parse("d76bef99-32b0-4aa0-96bf-d7cf4ac24321"))
            {
                Name = "IF"
            };

            _itemG = new YeetItem(Guid.Parse("18f2cdce-2f12-4478-93fa-8dbc3c0be65f"))
            {
                Name = "IG"
            };

            _itemI = new YeetItem(Guid.Parse("571d85f3-4d15-4440-bbe4-f33edbd71e5d"))
            {
                Name = "II"
            };

            _itemJ = new YeetItem(Guid.Parse("8247d73e-5700-45e2-a43c-d4d57ad5c52d"))
            {
                Name = "IJ"
            };

            _itemK = new YeetItem(Guid.Parse("e9b7ccc7-f9ca-42f0-85b1-5cf725886c8a"))
            {
                Name = "IK"
            };

            _itemX = new YeetItem(Guid.Parse("fbc4371d-fd63-4521-919e-23f79db642f2"))
            {
                Name = "IX"
            };

            _itemY = new YeetItem(Guid.Parse("25af784d-7fb1-449e-a308-94cef1d8ec48"))
            {
                Name = "IY"
            };

            _itemZ = new YeetItem(Guid.Parse("edde0bd5-0b73-468e-8ad9-c14043c1f35e"))
            {
                Name = "IZ"
            };
        }

        protected virtual void InitAggregates()
        {
            _library.Root = _root;
            _root.AddChild(_list1);
            _root.AddChild(_itemA);
            _root.AddChild(_itemB);
            _root.AddChild(_itemC);
            _list1.AddChild(_list2);
            _list1.AddChild(_itemE);
            _list1.AddChild(_itemF);
            _list1.AddChild(_itemG);
            _list2.AddChild(_list3);
            _list2.AddChild(_itemI);
            _list2.AddChild(_itemJ);
            _list2.AddChild(_itemK);
            _list3.AddChild(_itemX);
            _list3.AddChild(_itemY);
            _list3.AddChild(_itemZ);

            _libraries = new List<YeetLibrary<YeetList>>() { _library };
            _items = new List<YeetItem>() {
                _root, _itemA, _itemB, _itemC,
                _list1, _itemE, _itemF, _itemG,
                _list2, _itemI, _itemJ, _itemK,
                _list3, _itemX, _itemY, _itemZ
            };
            _lists = new List<YeetList>() {
                _root, _list1, _list2, _list3
            };
        }
    }
}