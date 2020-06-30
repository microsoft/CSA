![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png)


<div class="MCWHeader1">
Migrate EDW to Azure SQL Data Warehouse
</div>

<div class="MCWHeader2">
Whiteboard design session student guide
</div>

<div class="MCWHeader3">
October 2019
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2019 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Migrate EDW to Azure SQL Data Warehouse whiteboard design session student guide](#migrate-edw-to-azure-sql-data-warehouse-whiteboard-design-session-student-guide)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Step 1: Review the customer case study](#step-1-review-the-customer-case-study)
    - [Customer situation](#customer-situation)
    - [Customer needs](#customer-needs)
    - [Customer objections](#customer-objections)
    - [Infographic for common scenarios](#infographic-for-common-scenarios)
  - [Step 2: Design a proof of concept solution](#step-2-design-a-proof-of-concept-solution)
  - [Step 3: Present the solution](#step-3-present-the-solution)
  - [Wrap-up](#wrap-up)
  - [Additional references](#additional-references)

<!-- /TOC -->

# Migrate EDW to Azure SQL Data Warehouse whiteboard design session student guide

## Abstract and learning objectives

This whiteboard design session will look at the process of migrating an on-premises data warehouse to Azure SQL Data Warehouse. The design session will cover planning for a data warehouse migration, data and schema preparation, data loading, optimizing the data distribution, building a solution to support ad-hoc queries, migrating existing ETL packages and visualizing data with Power BI. 

At the end of this whiteboard design session, you will be better able to plan and implement a migration of your existing on-premises enterprise data warehouse to Azure SQL Data Warehouse and integrating it with both cloud-based and on-premises services and data sources.

## Step 1: Review the customer case study 

**Outcome**

Analyze your customer's needs.

Timeframe: 15 minutes

Directions: With all participants in the session, the facilitator/SME presents an overview of the customer case study along with technical tips.

1.  Meet your table participants and trainer.

2.  Read all of the directions for steps 1-3 in the student guide.

3.  As a table team, review the following customer case study.

### Customer situation

Coho is a retail company based in Austin, Texas that focuses on buying and selling mobile electronics and services. Coho has approximately 500 stores spread throughout the United States and a successful direct-to-consumer e-commerce site. Last year Coho began migrating to the cloud with development/test environments moving first and upgrading all store personnel to using Office 365 last year. This included configuring Azure Active Directory AD.

Coho sales data is a mix of online website orders and in-store point of sales data. Coho stores use a third-party point of sales system that writes transactional data to an operational data store in real-time. The Coho e-commerce site is built with .NET and uses SQL Server 2014 to store customer profiles and online order information. Sales data from both the Point of Sale (POS) system and the website are transferred every night via SQL Server Integration Service (SSIS) packages to a central data warehouse. In order to support all of the required report functionality, other data from various systems is loaded to the data warehouse as well. The most critical of these is the product load which loads all of the product information from the Product Master Data system which is updated by the business throughout the day. Due to the vast amount of ETL they currently have, they would like to minimize the amount of rewriting and limit ETL changes to connection strings and high-value transformations only. The data warehouse itself is currently hosted on SQL Server 2012. Frances Bradley, the Manager of Data Warehousing, states, "We have outgrown the current data warehouse. The system runs just fine most of the year but a few times per year the system is just stressed beyond the limit."

The customer analytics systems are very rudimentary and take too long to extract insights from the data. Sales and customer profile data are sent to a third-party data company once per day. The company then combines this sales data with its own information about the customer, cleanses and aggregates the raw customer data, and sends the curated results back to Coho the next day wherein the analytics and marketing teams can begin reviewing the data (typically using Excel). Obviously, this is taking a long time from start to finish. According to Jude Watkins, the Director of Customer Analytics, "We need to be able to react quicker. Our data is currently stored in a dedicated on-premises SQL Server for now but with the new Customer 360 project that is in pilot now we are going to capitalize on HDInsight to store and process the raw data ourselves. We sometimes pull up-to-date data from the warehouse but due to performance concerns on the warehouse side this is not done on a regular basis. We need our data to be near real-time. We also need access to warehouse data on a regular basis. A key pillar to this effort will be a data warehouse solution that can handle the analytics workloads on a daily basis, even on high traffic days." The upcoming "Customer 360" project will bring all of the third-party data processing and curation in-house, so Coho can do their own analytics as the data arrives instead being dependent on third-party processing.

The current reporting system is also quite limited. After the data loads have completed, the reports are generated and emailed out to all store managers and up. Reports are customized for the individual user's role and store/region. As part of the warehouse migration Coho would like to implement a self-serve BI platform to supplement the emailed reports. The current reports are very basic, mostly text. Reports primarily draw from the data warehouse sales data but also pull from some other SQL Servers and pull the critical Key Performance Indicator KPIs from SQL Server Analysis Server which is hosted on a separate machine from the warehouse. The business would like to see mobile access to their reports and dashboards while marketing and analytics want more advanced self-service BI and analytics features. The Vice President of Store Operations, Sloane Peterson, says, "Regional and store managers need to spend more time making their stores successful and less time pouring over spreadsheets in the back office. The current reports have valuable information, but this format is difficult to read on a mobile device and the data is not always clear. I want managers to have the same insights that we have at HQ."

**Current Coho EDW architecture**

![The Current Coho EDW architecture consists of an on-premises data warehouse that pulls data from a variety of on-premises data sources. A 3rd-party data processing company receives a dataset from Coho every day and supplements and aggregates the data with additional customer data. Updated sales data is periodically pulled from the warehouse but not on a consistent basis. Users receive static reports that are generated directly from the warehouse and analytics users query their own analytics database via Excel.](images/Whiteboarddesignsessionstudentguide-MigrateEDWtoAzureSQLDataWarehouseimages/media/image2.png)

### Customer needs 

1.  A data warehouse solution that allows Coho to scale to meet peak demand while keeping costs in check. At any point in time we may have 50+ queries running. We need to be able to handle this type of load. How do we choose appropriate performance levels?

2.  Create a process for continued integration with the e-commerce site and the on-premises data sources. They would like to minimize the amount of rewrite necessary for ETL/ELT processes to only high value changes. They have hundreds of SSIS packages they do not want to rewrite. How do we migrate these to Azure?

3.  Warehouse solution should integrate with the upcoming customer 360 project.

4.  Upgrade the existing reporting system with a solution that supports self-service BI.

5.  This warehouse contains Personally Identifiable Information. It is absolutely critical that this data is not exposed.

### Customer objections 

1.  We are concerned about spending too much to accommodate peak loads. We do not want to pay for a lot of excess warehouse capacity all year just to handle Black Friday. How will this solution help us control costs?

2.  We cannot take a week-long outage to perform the migration. How are we optimizing the migration?

3.  How will we integrate with existing analytics systems?

4.  We need to prepare for the customer 360 project. How will the new data warehouse solution integrate with HDInsight?

5.  We would like to have the ability to archive data from the warehouse, but we cannot have data completely offline.

6.  The self-service BI solution must be able to query the sales data in the warehouse as well as on-premises data sources. How will this system accomplish these goals?

7.  We have heard that Azure SQL Data Warehouse does not support geo-replication as in SQL Database. Help us understand how Azure protects our data in Azure SQL Data Warehouse.

8.  It looks like Azure SQL Database supports columnstore tables for warehouse workloads like ours. When would we choose Azure SQL Database over Azure SQL Data Warehouse?
 
### Infographic for common scenarios

**Azure SQL Data Warehouse**

![The SQL Data Warehouse Architecture diagram starts with a Control node, which connects to four Compute Nodes, which connects to Blob storage. At this time, we are unable to capture all of the information in the diagram. Future versions of this course should address this.](images/Whiteboarddesignsessionstudentguide-MigrateEDWtoAzureSQLDataWarehouseimages/media/image3.png)

**Azure Analysis Services**

![Screenshot of the Azure Analysis Services diagram. The diagram uses icons to illustrate the following features: Dynamic scalability;
Scale-out performance, ability to combine data from multiple data sources; Integrate with Data Factory; Leverage existing tabular data models.](images/Whiteboarddesignsessionstudentguide-MigrateEDWtoAzureSQLDataWarehouseimages/media/image4.png)

**Azure Data Factory**

![The Azure Data Factory overview starts on the left with severa Data Sources. This points to the steps Ingest, Prepare, Transform and Analyze, and then Publish. On the right side, Data Consumption points to Publish.](images/Whiteboarddesignsessionstudentguide-MigrateEDWtoAzureSQLDataWarehouseimages/media/image5.png)

**Power BI**

![The Power BI screenshot contains several comparative graphs about a Retail Store\'s Performance. At the top, left, a line graph has two lines comparing this year\'s and last year\'s sales records by month. Under this graph is a table listing the Top 10 products by rank and total sales. On the right side is a box graph (a box filled with smaller boxes). Boxes vary by size depending on their sales. Underneath the box graph is a bubble chart comparing Sales variances by percentages for Chain, Fashions Direct, and Lindseys. At the bottom are pictures of District Managers, and hyperlinks to more information about them.](images/Whiteboarddesignsessionstudentguide-MigrateEDWtoAzureSQLDataWarehouseimages/media/image6.png)

## Step 2: Design a proof of concept solution

**Outcome**

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 60 minutes

**Business needs**

Directions:  With all participants at your table, answer the following questions and list the answers on a flip chart:

1.  Who should you present this solution to? Who is your target customer audience? Who are the decision makers?

2.  What customer business needs do you need to address with your solution?

**Design**

Directions: With all participants at your table, respond to the following questions on a flip chart:

*Plan a data warehouse migration*

1. **Data preparation**: Coho needs to validate its database compatibility. Develop a high-level checklist of all of the steps necessary to validate compatibility including any tools you might use.

2.  **Data warehouse sizing:** Determine the appropriate sizing/performance configuration for the data warehouse.

3. **Migration process:** Design a secure migration process that minimizes downtime. Your migration process should include the following:

    a.  Recommendations for migrating and validating schema

    b.  Recommendations for migrating and validating data

    c.  Recommendations for migrating and validating code

    d.  Architectural considerations that may be necessary for running on SQL Data Warehouse

4. **Post migration steps:** Define any post migration steps that should be run to prepare the database.

5. **Diagram the solution**

*Data warehouse integration*

1. **Plan**: Identify integration points:

    a.  How will data from the warehouse be integrated with data in HDInsight?

    b.  Which Extract, Transform, and Load (ETL) process will be migrated?

    c.  What changes will be necessary for ETL that are out of scope of this project?

    d.  How do they lift and shift SSIS packages for execution in Azure?

2. **Connecting with existing systems**: Provide the configuration details for setting up connectivity back to on-premises data sources.

3. **Diagram the solution**

*Self-service BI*

1. **Describe self-service BI:**

    a.  How does the solution meet the mobile requirements?

    b.  How does this design meet the stated security goals?

    c.  We are trying to minimize costs. How many licenses would we need to purchase?

2. **Provide the following configuration details:**

    a.  How will this solution connect back to the on-premises data sources?

    b.  How will users be licensed/given access to Power BI?

    c.  What impact will this solution have on the performance of the Azure SQL Data Warehouse?
    
3. **Diagram the solution**

**Prepare**

Directions: With all participants at your table:

1.  Identify any customer needs that are not addressed with the proposed solution.

2.  Identify the benefits of your solution.

3.  Determine how you will respond to the customer's objections.

Prepare a 15-minute chalk-talk style presentation to the customer.

## Step 3: Present the solution

**Outcome**

Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation**

Directions:

1.  Pair with another table.

2.  One table is the Microsoft team and the other table is the customer.

3.  The Microsoft team presents their proposed solution to the customer.

4.  The customer makes one of the objections from the list of objections.

5.  The Microsoft team responds to the objection.

6.  The customer team gives feedback to the Microsoft team.

7.  Tables switch roles and repeat Steps 2-6.

##  Wrap-up 

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

##  Additional references

|    |            |
|----------|:-------------:|
| **Description** | **Links** |
|  Azure SQL Data Warehouse  |  <https://docs.microsoft.com/en-us/azure/sql-data-warehouse/sql-data-warehouse-overview-what-is>   |
|  Azure Database Migration Guide | <https://datamigration.microsoft.com/scenario/sql-to-sqldw?step=1>  |
|  Load data into Azure SQL Data Warehouse | <https://azure.microsoft.com/en-us/documentation/articles/sql-data-warehouse-overview-load/>  |
|  Manage tables and indexes in Azure SQL Data Warehouse |  <https://azure.microsoft.com/en-us/documentation/articles/sql-data-warehouse-overview-manage-tables-indexes/>  |
| Leverage other services with SQL Data Warehouse   | <https://azure.microsoft.com/en-us/documentation/articles/sql-data-warehouse-overview-integrate/>      |
| Copy data from an on-premises SQL Server database to Azure Blob storage | <https://docs.microsoft.com/en-us/azure/data-factory/tutorial-hybrid-copy-portal>    |
| Azure Analysis Services  | <https://docs.microsoft.com/en-us/azure/analysis-services/analysis-services-overview>   |
| Power BI Gateway   | <https://powerbi.microsoft.com/en-us/gateway/>   |

