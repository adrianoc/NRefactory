﻿// 
// RedundantElseIssueTests.cs
// 
// Author:
//      Mansheng Yang <lightyang0@gmail.com>
// 
// Copyright (c) 2012 Mansheng Yang <lightyang0@gmail.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using ICSharpCode.NRefactory.CSharp.Refactoring;
using NUnit.Framework;

namespace ICSharpCode.NRefactory.CSharp.CodeIssues
{
	[TestFixture]
	public class RedundantElseIssueTests : InspectionActionTestBase
	{

		[Test]
		public void TestReturn ()
		{
			var input = @"
class TestClass
{
	int TestMethod (int i)
	{
		if (i > 0)
			return 1;
		else
			return 0;
	}
}";
			var output = @"
class TestClass
{
	int TestMethod (int i)
	{
		if (i > 0)
			return 1;
		return 0;
	}
}";
			Test<RedundantElseIssue> (input, 1, output);
		}

		[Test]
		public void TestBreakLoop ()
		{
			var input = @"
class TestClass
{
	void TestMethod ()
	{
		int k = 0;
		for (int i = 0; i < 10; i++) {
			if (i > 5)
				break;
			else
				k++;
		}
	}
}";
			var output = @"
class TestClass
{
	void TestMethod ()
	{
		int k = 0;
		for (int i = 0; i < 10; i++) {
			if (i > 5)
				break;
			k++;
		}
	}
}";
			Test<RedundantElseIssue> (input, 1, output);
		}

		[Test]
		public void TestContinueLoop ()
		{
			var input = @"
class TestClass
{
	void TestMethod ()
	{
		int k = 0;
		for (int i = 0; i < 10; i++) {
			if (i > 5)
				continue;
			else
				k++;
		}
	}
}";
			var output = @"
class TestClass
{
	void TestMethod ()
	{
		int k = 0;
		for (int i = 0; i < 10; i++) {
			if (i > 5)
				continue;
			k++;
		}
	}
}";
			Test<RedundantElseIssue> (input, 1, output);
		}

		[Test]
		public void TestBlockStatement()
		{
			var input = @"
class TestClass
{
	int TestMethod (int i)
	{
		if (i > 0) {
			return 1;
		} else {
			return 0;
		}
	}
}";
			var output = @"
class TestClass
{
	int TestMethod (int i)
	{
		if (i > 0) {
			return 1;
		}
		return 0;
	}
}";
			Test<RedundantElseIssue> (input, 1, output);
		}

		[Test]
		public void TestEmptyFalseBlock ()
		{
			var input = @"
class TestClass
{
	void TestMethod (int i)
	{
		int a;
		if (i > 0)
			a = 1;
		else { }
	}
}";
			var output = @"
class TestClass
{
	void TestMethod (int i)
	{
		int a;
		if (i > 0)
			a = 1;
	}
}";
			Test<RedundantElseIssue> (input, 1, output);
		}

		[Test]
		public void TestNecessaryElse ()
		{

			var input = @"
class TestClass
{
	void TestMethod (int i)
	{
		int a;
		if (i > 0)
			a = 1;
		else
			a = 0;
	}
}";
			Test<RedundantElseIssue> (input, 0);
		}

		[Test]
		public void TestNecessaryElseCase2 ()
		{

			var input = @"
class TestClass
{
	void TestMethod (int i)
	{
		int a;
		while (true) {
			if (i > 0) {
				a = 1;
			} else if (i < 0) {
				a = 0;
				break;
			} else {
				break;
			}
		}
	}
}";
			Test<RedundantElseIssue> (input, 0);
		}
	}
}
