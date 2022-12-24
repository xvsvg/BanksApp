# BanksApp
Three layer banks simulation

There different banks, where you can create your account.

<h3> Accounts </h3>

* Credit — fixed charge rate, provided by a bank, and no interest rate. Overdraft and expiration date are optional.
* Debit — fixed interest rate, provided by a bank, and no charge rate. Overdraft and expiration date are optional.
* Deposit — interest rate depends on first deposit, no charge rate, all withdraw transaction enabled, when account is expired. Overdraft is optional.

<h3> Operations </h3> 

* Withdraw
* Deposit
* Transaction between several accounts

<h3> Other information </h3>

Bank has a local time and accelerated time. The last one is a mechanism of a time acceleration to find out, what would be with account money
in a certain time stamp. Only russian bank accounts implemented.

<h3> Tools </h3>

* .NET 6
* Spectre.Console 0.45
* Xunit 2.4
