using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace YeetOverFlow.Core.Application.Tests
{
	//[MethodUnderTest]_With/Of_[Scenario]_Should_[ExpectedResult]
	public class TestOutputTests : YeetTestBase
    {
		Regex _regexMethodMatch = new Regex(@"_With(?<method>.*?)_");

		[SetUp]
		public override void Setup()
		{
			InitVariables();
		}

		private void Init(String methodName)
        {
            Regex expression = new Regex(@"(?<L1>L1(Left|Middle|Right))(?<L2>L2(Left|Middle|Right))(?<L3>L3(Left|Middle|Right))");

            Match match = expression.Match(methodName);
			if (match.Success)
			{
				string l1 = match.Groups["L1"].Value;
				string l2 = match.Groups["L2"].Value;
				string l3 = match.Groups["L3"].Value;

				if (l1 == "L1Left") _root.AddChild(_list1);
				_root.AddChild(_itemA);
				_root.AddChild(_itemB);
				if (l1 == "L1Middle") _root.AddChild(_list1);
				_root.AddChild(_itemC);
				if (l1 == "L1Right") _root.AddChild(_list1);
				if (l2 == "L2Left") _list1.AddChild(_list2);
				_list1.AddChild(_itemE);
				_list1.AddChild(_itemF);
				if (l2 == "L2Middle") _list1.AddChild(_list2);
				_list1.AddChild(_itemG);
				if (l2 == "L2Right") _list1.AddChild(_list2);
				if (l3 == "L3Left") _list2.AddChild(_list3);
				_list2.AddChild(_itemI);
				_list2.AddChild(_itemJ);
				if (l3 == "L3Middle") _list2.AddChild(_list3);
				_list2.AddChild(_itemK);
				if (l3 == "L3Right") _list2.AddChild(_list3);
				_list3.AddChild(_itemX);
				_list3.AddChild(_itemY);
				_list3.AddChild(_itemZ);
			} 
        }

		[Test]
		public void TestOutput_WithL1LeftL2LeftL3Left_ShouldDisplayL1LeftL2LeftL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2MiddleL3Left_ShouldDisplayL1LeftL2MiddleL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2RightL3Left_ShouldDisplayL1LeftL2RightL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2LeftL3Middle_ShouldDisplayL1LeftL2LeftL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2MiddleL3Middle_ShouldDisplayL1LeftL2MiddleL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2RightL3Middle_ShouldDisplayL1LeftL2RightL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2LeftL3Right_ShouldDisplayL1LeftL2LeftL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2MiddleL3Right_ShouldDisplayL1LeftL2MiddleL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1LeftL2RightL3Right_ShouldDisplayL1LeftL2RightL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2LeftL3Left_ShouldDisplayL1MiddleL2LeftL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2MiddleL3Left_ShouldDisplayL1MiddleL2MiddleL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2RightL3Left_ShouldDisplayL1MiddleL2RightL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2LeftL3Middle_ShouldDisplayL1MiddleL2LeftL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2MiddleL3Middle_ShouldDisplayL1MiddleL2MiddleL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2RightL3Middle_ShouldDisplayL1MiddleL2RightL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2LeftL3Right_ShouldDisplayL1MiddleL2LeftL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2MiddleL3Right_ShouldDisplayL1MiddleL2MiddleL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1MiddleL2RightL3Right_ShouldDisplayL1MiddleL2RightL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2LeftL3Left_ShouldDisplayL1RightL2LeftL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2MiddleL3Left_ShouldDisplayL1RightL2MiddleL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2RightL3Left_ShouldDisplayL1RightL2RightL3Left()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2LeftL3Middle_ShouldDisplayL1RightL2LeftL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2MiddleL3Middle_ShouldDisplayL1RightL2MiddleL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2RightL3Middle_ShouldDisplayL1RightL2RightL3Middle()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2LeftL3Right_ShouldDisplayL1RightL2LeftL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2MiddleL3Right_ShouldDisplayL1RightL2MiddleL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestOutput_WithL1RightL2RightL3Right_ShouldDisplayL1RightL2RightL3Right()
		{
			string methodName = new StackTrace().GetFrame(0).GetMethod().Name;
			methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;
			Init(methodName);
			string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");
			TestContext.Out.WriteLine(expected);
			string actual = TestHelper.TestOutput(_root);
			Assert.AreEqual(expected, actual);
		}
	}
}
