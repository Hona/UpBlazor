<div align="center">

  ![UpBlazor](./_docs/img/logo.png)

  ![.NET Build + Test](https://github.com/Hona/UpBlazor/actions/workflows/dotnet.yml/badge.svg)
  
</div>

## What is this?

🏦 I built this site to integrate with [Up bank](https://up.com.au) (an Australian digital bank), to assist users with **budgeting** and to gain **powerful future insights**.

### Epics

 - [ ] ⚡ Leverage the [Up API](https://developer.up.com.au) (using [Up.NET](https://github.com/Hona/Up.NET)) to:

     - [x] Fetch existing Transactional and Saver accounts to make selecting which account to save/expense simple
     - [x] Use existing account balances for more accurate insights
     - [ ] Track actual vs intended savings
     - [ ] Automatically set savings in app when created on the site (this API does not exist on Up yet...)
    
 - [x] 💲 Track incomes

     - [x] Exact ($)
     - [x] Allow multiple

 - [x] 🧾 Track expenses
 
     - [x] Exact ($) or Relative (%) cost
     - [x] Source from Income streams/Up accounts
     - [x] One off + Recurring

 - [x] 🪣 Create savings plans
 
     - [x] Create multiple, per income
     - [x] Exact ($) or Relative (%) cost
     - [x] Choose which Saver account to put the amount into

 - [ ] 📈 Insights + analytics

     - [x] Show a breakdown of how the calculations work (relative -> absolute), rolling totals, etc
     - [ ] Forecast graphs

        - [x] Income (in each Up account) - account for all income streams
        - [x] Expenses (aggregate recurring and one offs)
        - [ ] Net Balances (Income - Expenses, in each account)

     - [ ] Suggested Budgetting
     - [ ] Daily Breakdown
     - [ ] Actual vs Intended
     - [ ] Reports
     
<div align="center">
     
## Examples

#### Layout + Notifications

![Layout + Notifications](./_docs/img/notifications.png)

#### Graph

![Graph](./_docs/img/graph.png)

#### Responsive Sider

<img src="./_docs/img/responsive-sider.png" width="50%" />

#### Form

![Form](./_docs/img/form-components.png)

#### Result

![Result](./_docs/img/status-with-form.png)

#### Table

![Table](./_docs/img/table.png)

</div>

## Code Architecture

This project follows a simplistic take on Clean Architecture.

#### UpBlazor.Core

* Contains the core models that are stored in the database/not dependent on anything
* Repository interfaces to abstract the infrastructure layer
* Helper methods/extensions
* Services

#### UpBlazor.Infrastructure

* Contains repository implementations using [Marten DB](https://martendb.io/)

#### UpBlazor.Web

* Frontend is Blazor server-side
* Authentication is using Microsoft OAuth2
* UI Framework is AntBlazor, with AntBlazor Charts (based on G2Plot)

## Getting Started

### Development

1. Install a local instance of Postgres
2. Create a database called upblazor, and a user with access
3. Update the Marten connection string in appsettings.json
4. Build and run UpBlazor.Web

### Production

1. Install Docker and docker-compose
2. Run `docker-compose up -d --build`
3. The program is exposed on port 9994, so reverse proxy your domain to that port

## Acknowledgments

* [AntBlazor](https://github.com/ant-design-blazor/ant-design-blazor)
* [Up bank](https://up.com.au)

## Contributors

<table>
<tr>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/Hona>
            <img src=https://avatars.githubusercontent.com/u/10430890?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Luke Parker [SSW]/>
            <br />
            <sub style="font-size:14px"><b>Luke Parker [SSW]</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/anddrzejb>
            <img src=https://avatars.githubusercontent.com/u/6518006?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Andrzej Bakun/>
            <br />
            <sub style="font-size:14px"><b>Andrzej Bakun</b></sub>
        </a>
    </td>
</tr>
</table>
