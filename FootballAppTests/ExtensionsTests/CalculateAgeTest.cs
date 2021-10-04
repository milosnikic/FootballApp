using FootballApp.API.Helpers;
using System;
using Xunit;
using Xunit.Abstractions;

namespace FootballAppTests
{
    public class CalculateAgeTest
    {
        public readonly ITestOutputHelper _output;

        public CalculateAgeTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TodayAgeShouldBeMinusOne()
        {
            var age = Extensions.CalculateAge(DateTime.Now);
            Assert.Equal(-1, age);
        }

        [Fact]
        public void ProperDateTimeCheck()
        {
            var date = DateTime.Now.AddDays(-1).AddYears(-15);
            var age = Extensions.CalculateAge(date);
            _output.WriteLine(date.ToString());
            Assert.Equal(15, age);
        }

        [Theory]
        [MemberData(nameof(CorrectData))]
        public void InvalidDateTimeShouldBeNullTheory(int expected, DateTime date)
        {
            _output.WriteLine($"Expected: {expected}");
            _output.WriteLine($"Date: {date}");
            Assert.Equal(expected, Extensions.CalculateAge(date));
        }

        public static readonly object[][] CorrectData =
        {
            new object[] { -1, DateTime.Now.AddYears(1) },
            new object[] { -1, DateTime.MaxValue }
        };

    }
}
