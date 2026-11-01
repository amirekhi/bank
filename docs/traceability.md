# Traceability: rule ID to tests

Every rule in the Business Logic Specification v1.0 gets at least one named test.
This file grows with each phase. A rule that is not listed here has no test yet.

## Phase 1: building blocks

| Rule | What it says | Tests |
|---|---|---|
| 5.1 | Money has at most 2 decimals, is rejected not rounded, signed, bounded | `MoneyTests` (Of_*, Money_is_signed_*, Addition_beyond_the_maximum_*) |
| P-12 | Accrual 6 decimals, posting rounds half to even to 2 decimals | `MoneyTests.RoundHalfEven_*`, `InterestRateTests.Accrual_always_has_at_most_six_decimals` |
| P-11, I-02, I-03 | Actual/365 fixed daily accrual on the balance magnitude | `InterestRateTests.Daily_*` |
| I-06 (maths) | Monthly posting rounds, residual carried | `InterestRateTests.Savings_month_*`, `Overdraft_month_*`, `A_changing_balance_*` |
| P-24, T-07 | Business date is the UTC date of the timestamp | `BusinessDateTests.Business_date_is_the_UTC_*`, `A_timestamp_with_an_offset_*`, `Today_reads_*` |
| E-04 (month end) | Last day of month detection incl. leap years | `BusinessDateTests.IsLastDayOfMonth_*` |
| A-08, P-29 | Account number: 9-digit sequence + Luhn check digit | `LuhnTests`, `AccountNumberTests` |
| S-01, P-21 | Idempotency key 8 to 64 safe characters | `IdempotencyKeyTests` |
| P-22 | Reason 10 to 500 characters after trimming | `ReasonTests` |
| T-06 | Posting reference PST-yyyymmdd-nnnnnn | `PostingReferenceTests` |
