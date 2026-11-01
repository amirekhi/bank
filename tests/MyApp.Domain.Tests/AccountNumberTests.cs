using MyApp.Domain.Common;
using Shouldly;
using Xunit;

namespace MyApp.Domain.Tests;

public class LuhnTests
{
    // 79927398713 is the standard published Luhn test number.
    [Fact]
    public void Check_digit_matches_the_standard_test_vector()
        => Luhn.ComputeCheckDigit("7992739871").ShouldBe(3);

    [Fact]
    public void IsValid_accepts_a_correct_number_and_rejects_a_wrong_digit()
    {
        Luhn.IsValid("79927398713").ShouldBeTrue();
        Luhn.IsValid("79927398714").ShouldBeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("7")]
    [InlineData("7992739871x")]
    [InlineData("7992 739871")]
    public void IsValid_rejects_malformed_input(string? input)
        => Luhn.IsValid(input).ShouldBeFalse();

    [Fact]
    public void ComputeCheckDigit_rejects_non_digits()
        => Should.Throw<ValidationFailedException>(() => Luhn.ComputeCheckDigit("12a4"));
}

public class AccountNumberTests
{
    [Fact]
    public void FromSequence_appends_one_check_digit_to_the_nine_digit_sequence()
    {
        AccountNumber.FromSequence(100_000_000).Value.ShouldBe("1000000008");
        AccountNumber.FromSequence(999_999_999).Value.ShouldBe("9999999999");
    }

    [Theory]
    [InlineData(99_999_999L)]
    [InlineData(1_000_000_000L)]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void FromSequence_rejects_sequences_outside_the_nine_digit_range(long sequence)
        => Should.Throw<ValidationFailedException>(() => AccountNumber.FromSequence(sequence));

    [Fact]
    public void Every_generated_number_is_ten_digits_and_passes_the_Luhn_check()
    {
        var random = new Random(7);

        for (var i = 0; i < 1000; i++)
        {
            var sequence = random.NextInt64(AccountNumber.FirstSequence, AccountNumber.LastSequence + 1);
            var number = AccountNumber.FromSequence(sequence);

            number.Value.Length.ShouldBe(10);
            Luhn.IsValid(number.Value).ShouldBeTrue();
            AccountNumber.Parse(number.Value).ShouldBe(number);
        }
    }

    [Fact]
    public void Changing_any_single_digit_makes_the_number_invalid()
    {
        var random = new Random(11);

        for (var i = 0; i < 200; i++)
        {
            var original = AccountNumber.FromSequence(
                random.NextInt64(AccountNumber.FirstSequence, AccountNumber.LastSequence + 1)).Value;

            var position = random.Next(0, original.Length);
            var replacement = (char)('0' + (original[position] - '0' + random.Next(1, 10)) % 10);

            var mutated = original.Remove(position, 1).Insert(position, replacement.ToString());

            AccountNumber.TryParse(mutated, out _).ShouldBeFalse($"{original} -> {mutated}");
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("100000000")]
    [InlineData("10000000081")]
    [InlineData("100000000x")]
    [InlineData("1000000009")]
    [InlineData("0000000000")]
    public void Parse_rejects_wrong_length_non_digits_bad_check_digit_and_a_leading_zero(string? input)
        => Should.Throw<ValidationFailedException>(() => AccountNumber.Parse(input));

    [Fact]
    public void Account_numbers_are_equal_by_value()
        => AccountNumber.FromSequence(100_000_001).ShouldBe(AccountNumber.FromSequence(100_000_001));
}
