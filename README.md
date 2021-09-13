<div align="center">

  # ![UpBlazor](./_docs/img/logo.png)

</div>

## What is this?

I built this site to integrate with [Up bank](https://up.com.au) (an Australian digital bank), to assist users with **budgetting** and to gain **powerful future insights**.

### Epics

 - [ ] Leverage the [Up API](https://developer.up.com.au) (using [Up.NET](https://github.com/Hona/Up.NET)) to:

     - [x] Fetch existing Transactional and Saver accounts to make selecting which account to save/expense simple
     - [x] Use existing account balances for more accurate insights
     - [ ] Track actual vs intended savings
     - [ ] Automatically set savings in app when created on the site (this API does not exist on Up yet...)
    
 - [x] Track incomes

     - [x] Exact ($)
     - [x] Allow multiple

 - [x] Track expenses
 
     - [x] Exact ($) or Relative (%) cost
     - [x] Source from Income streams/Up accounts
     - [x] One off + Recurring

 - [x] Create savings plans
 
     - [x] Create multiple, per income
     - [x] Exact ($) or Relative (%) cost
     - [x] Choose which Saver account to put the amount into

 - [ ] Insights + analytics

     - [x] Show a breakdown of how the calculations work (relative -> absolute), rolling totals, etc
     - [ ] Forecast graphs

        - [x] Income (in each Up account) - account for all income streams
        - [x] Expenses (aggregate recurring and one offs)
        - [ ] Net Balances (Income - Expenses, in each account)

     - [ ] Suggested Budgetting
     - [ ] Daily Breakdown
     - [ ] Actual vs Intended
     - [ ] Reports

## Examples

