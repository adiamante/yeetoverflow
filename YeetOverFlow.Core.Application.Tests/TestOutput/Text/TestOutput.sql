
declare @t0 table(val varchar(50))
declare	@l1 table(l1 varchar(50))
declare	@l2 table(l2 varchar(50))
declare	@l3 table(l3 varchar(50))

insert into @t0 values ('Left'),('Middle'),('Right')
insert into @l1 values ('L1')
insert into @l2 values ('L2')
insert into @l3 values ('L3')

select *, l1 + lx1.val + l2 + lx2.val + l3 + lx3.val as name,
	'[Test]' + CHAR(13) + CHAR(10) +
	'public void TestOutput_With' + l1 + lx1.val + l2 + lx2.val + l3 + lx3.val + '_ShouldDisplay' + l1 + lx1.val + l2 + lx2.val + l3 + lx3.val + '()' +  CHAR(13) + CHAR(10) +
	+ '{' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'string methodName = new StackTrace().GetFrame(0).GetMethod().Name;' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'methodName = _regexMethodMatch.Match(methodName).Groups["method"].Value;' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'Init(methodName);' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'string expected = File.ReadAllText($"TestOutput/Text/{methodName}.txt");' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'TestContext.Out.WriteLine(expected);' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'string actual = TestHelper.TestOutput(_root);' + CHAR(13) + CHAR(10) +
	CHAR(9) + 'Assert.AreEqual(expected, actual);' + CHAR(13) + CHAR(10) +
	'}' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
	AS TestMethod,
	'copy NUL ' + l1 + lx1.val + l2 + lx2.val + l3 + lx3.val + '.txt' AS CreateFile
from (
	select *
	from @l1
	cross apply @t0
) lx1
cross apply
(
	select *
	from @l2
	cross apply @t0
) lx2
cross apply
(
	select *
	from @l3
	cross apply @t0
) lx3