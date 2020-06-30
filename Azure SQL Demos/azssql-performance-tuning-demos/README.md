Tutorial: Performance Tuning for Azure SQL Databases
====================================================

In this repo, I will present considerations for monitoring Azure SQL
Databases using Metrics and diagnostics logging, Query performance
insight, Azure SQL Analytics for Advance performance Monitoring, and
contrast this with On-premise vs PaaS approaches.

Overview

1.  On-Premise versus PaaS model for DB Monitoring

2.  Metrics and Diagnostic Logging

3.  Using Query Performance Insights for Basic Performance Monitoring

4.  Using Azure SQL Analytics for Advance Performance Monitoring

5.  Tuning Azure SQL Performance Manually

On-Premise vs PaaS Model for Database Monitoring and Troubleshooting
--------------------------------------------------------------------

+----------------------------------+----------------------------------+
| **On-Premises**                  | **PaaS Model**                   |
+==================================+==================================+
| -   May be time-consuming        | -   No full administrative       |
|                                  |     control                      |
+----------------------------------+----------------------------------+
| -   Requires expertise in the    | -   Automated Performance        |
|     subject                      |     Monitoring and               |
|                                  |     recommendations              |
+----------------------------------+----------------------------------+
| -   Extensive set of tools for   | -   Extensive set of charts      |
|     monitoring and               |     available to simplify        |
|     troubleshooting, such as SQL |     analysis                     |
|     Profile and SQL Trace        |                                  |
+----------------------------------+----------------------------------+
| -   Fully manual analysis        | -   Built-in Artificial          |
|                                  |     Intelligence to support your |
|                                  |     analysis                     |
+----------------------------------+----------------------------------+
| -   No Graphical User Interface  | -   Automated Tuning             |
|     available                    |                                  |
+----------------------------------+----------------------------------+
|                                  | -   Automated Scalability        |
+----------------------------------+----------------------------------+

Metrics & Diagnostic Logging
----------------------------

Metrics are collected every minute and retained for 90 days. We can also
send to Log Analytics Workspace, send to event hub, or archiving them to
Azure Storage.

 

### Available Monitoring Telemetry for Databases

Telemetry definition - the process of recording and transmitting the
readings of an instrument

Basic Metrics - DTU, CPU percentage, physical data read percentage and
more

  **Diagnostics Logs**             **Metrics**
  -------------------------------- -------------
  SQLInsights                      Basic
  Automatic Tuning                 
  Query Store runtime statistics   
  Query Store wait statistics      
  Errors                           
  Database wait statistics         
  Timeouts                         
  Blocks                           
  Deadlocks                        

We can analyze the metric logs, visualize and aggregate by time periods,
trigger alerts to mitigate issues as they occur, automate with autoscale
to scale up or down as needed, export logs and metrics in storage for
export, or we can retrieve using powershell, API, CLI (command line
interface), etc.

 

 Can also use Kusto query language to do a deep analysis of the root
logs

#### Azure Monitor Metrics Vs Diagnostics Logs

  **Metrics**                                                            **Diagnostic Logs**
  ---------------------------------------------------------------------- ----------------------------------------------------
  Numerical values only                                                  Text or numeric data
  Collected at regular intervals                                         Analyzed with rich query language
  Ideal for fast detection of issues                                     Ideal for deep analysis and identifying root cause
  Lightweight and capable of near-real time scenarios such as alerting   View them on Azure Log Analytics
  View them on Azure Portal - Metrics Explorer                           

Azure Monitor Alert - can get notification when important conditions are
found in your monitoring data

 

**4 Main Alert Components**

-   Target Resource

-   Condition (metrics, log, components we want to track) specify a
    > value such as greater than a threshold or dynamic

-   Actions - we want to perform when the alert fires. Defined by action
    > groups (send email, trigger logic app, and more)

-   Alert details - define name, description, etc.

Using Query Performance Insights for Basic Performance Monitoring
-----------------------------------------------------------------

Query performance insight is a feature in azure database that helps us
understand the impact of queries in DB and how it relates to consumption
of resources

-   Basic performance tuning and troubleshooting from SQL DB Advisor

-   Requires a few hours of historic data

-   Relies on the Query Store

The features are:

-   Review top CPU-consuming queries

-   Review top queries per duration

-   View individual query details

-   Understand performance tuning annotations

### How To View Query Performance Insights

Go to Database and under intelligence performance look at Query
Performance Insight blades

-   Shows the aggregated DTU in the time period specified

-   See top 5 Queries with most consumption in DB

-   Can go to customer tab and select different values we want (10 top
    > queries)

```{=html}
<!-- -->
```
-    

>  

 

![](.//media/image1.png){width="7.699566929133859in"
height="3.584905949256343in"}

 

 

Can view details of the query by selecting one

-   Can see all the consumption of DTU and time for the query

-   List of all execution of this query and the different values
    > associated

-   Basic way to troubleshoot and monitor the database in terms of
    > performance

![](.//media/image2.png){width="6.5in" height="4.686805555555556in"}

 

 

Using Azure SQL Analytics for Advance Performance Monitoring
------------------------------------------------------------

Advance monitoring scenarios to collect and visualize important data
(for SQL Single Database, Elastic Pools, and MI).

-   Requires a Log Analytics Workspace

-   Support Alerts to proactive monitoring

-   Requires Diagnostic Settings to be configured in your databases

-   Provides relevant performance data and charts (DB errors, timeouts,
    query durations / waits)

 

Intelligent Insights - provides AI, provides continuous db monitoring
and relies on logs in SQLInsights

-   Can detect temporary deviations of workload

-   Perform root cause analysis with remediation options

### Detectable Database Performance Patterns Using Intelligence Insights

+------------------------------+--------------------------------------+
| -   Reaching resource Limits | -   Text or numeric data             |
+==============================+======================================+
| -   Workload Increase        | -   Analyzed with rich query         |
|                              |     language                         |
+------------------------------+--------------------------------------+
| -   Memory Pressure          | -   Ideal for deep analysis and      |
|                              |     identifying root cause           |
+------------------------------+--------------------------------------+
| -   Locking                  | -   View them on Azure Log Analytics |
+------------------------------+--------------------------------------+
| -   Increased MAXDOP         |                                      |
+------------------------------+--------------------------------------+
| -   Pagelatch contention     |                                      |
+------------------------------+--------------------------------------+
| -   Missing index            |                                      |
+------------------------------+--------------------------------------+
| -   New Query                |                                      |
+------------------------------+--------------------------------------+
| -   Increased wait statistic |                                      |
+------------------------------+--------------------------------------+

![](.//media/image3.png){width="6.5in" height="4.213888888888889in"}

Each source to Azure Diagnostics groups the logs from multiple source
and send to specific targets.

### Query Performance Insight vs Azure SQL Analytics

+----------------------------------+----------------------------------+
| **Query Performance Insight**    | **Azure SQL Analytics**          |
+==================================+==================================+
| -   Built-in feature with your   | -   Requires Log Analytics       |
|     Azure SQL database           |     Workspace with database      |
|                                  |     telemetry available          |
+----------------------------------+----------------------------------+
| -   Requries Query Store enabled | -   Requires Azure SQL Analytics |
|                                  |     workspace                    |
+----------------------------------+----------------------------------+
| -   Charts with relevant         | -   Automated database           |
|     performance metrics and      |     performance monitoring using |
|     resources consumption        |     Intelligent Insights         |
+----------------------------------+----------------------------------+
| -   Visualize individual queries | -   Extensive GUI to easily      |
|     resources consumption and    |     perform advanced performance |
|     performance                  |     analysis                     |
+----------------------------------+----------------------------------+
| -   GUI to easily perform basic  | -   Single place to monitor and  |
|     performance analysis         |     troubleshoot performance of  |
|                                  |     multiple databases           |
+----------------------------------+----------------------------------+
| -   Uses Azure Database Advisor  |                                  |
|     to provide performance       |                                  |
|     recommendations              |                                  |
+----------------------------------+----------------------------------+

### How To Create an Azure SQL Analytics Workspace

Go to portal and select "Log Analytics Workspace" or from Azure SQL
Analytics click on "Create New Workspace" as shown below

![](.//media/image4.png){width="6.5in" height="4.117361111111111in"}

After the workspace have been created, click the workspace and from the
Overview page, view the Summary to see all the solutions available.

-   Data must be added to view items here

-   Need a few hours to populate

-   Click the Database to see the default view

    -   Can view timeouts, query durations, deadlocks, errors, etc

![](.//media/image5.png){width="6.5in" height="3.6256944444444446in"}

![](.//media/image6.png){width="6.5in" height="2.9118449256342958in"}

Click on Resources with performance insights to get Intelligence
Insights (below is a default view).

![](.//media/image7.png){width="6.5in" height="2.4618055555555554in"}

Next select the database and we get another view below of all insights
related to database

-   Below we see queries causing locks, high consumption of CPU (can
    > click on query)

![](.//media/image8.png){width="6.5in" height="3.0631944444444446in"}

Select query and we get the details of the query

![](.//media/image9.png){width="6.5in" height="2.79375in"}

Tuning Azure SQL Performance Manually
-------------------------------------

Before Starting to Manually tune your database

-   Automatic Tuning - use built in AI and monitor DB to identify
    > performance issues. Then actions are enumerated to fix

-   Performance Recommendations feature - Built in to provide
    > recommendations to improve performance

-   Query Performance Insight - helps monitor and identify issues in DB
    > so we can perform actions to mitigate DB issues

-   Azure SQL Analytics - helps to monitor and troubleshoot DB
    > performance in more advance scenarios

![](.//media/image10.png){width="6.5in" height="3.011111111111111in"}

### Features Available per Database Service

+----------------------------------+----------------------------------+
| **Azure SQL Database / Elastic   | **Managed Instance Database**    |
| Pool**                           |                                  |
+==================================+==================================+
| -   Automatic Tuning             | -   Automatic Tuning (only       |
|                                  |     detects Force Last Good      |
|                                  |     Plan)                        |
+----------------------------------+----------------------------------+
| -   Performance Recommendations  | -   SQL Analytics                |
+----------------------------------+----------------------------------+
| -   Query Performance Insights   |                                  |
+----------------------------------+----------------------------------+
| -   SQL Analytics                |                                  |
+----------------------------------+----------------------------------+

Performance Recommendations - enabled by default and access / analyze
usage of DB and provides recommendations.

![](.//media/image11.png){width="6.5in" height="2.6590277777777778in"}

The following group of recommendations are provided

-   Drop indexes are not compatible in situations using partition
    > switching or index hints

-   Dropping unused index are not available for premium or business
    > critical service tiers

-   Parameterize queries recommendations - scenarios where queries
    > running against DB constantly being recompiled and using the same
    > query execution plan. We will have recommendations to parameterize
    > so they are not recompiled constantly

![](.//media/image12.png){width="6.5in" height="3.1305555555555555in"}

### How To View Performance Recommendations

Go to Azure SQL Database and under Intelligence Performance go to
Performance Recommendations

-   You get a list of all recommendations tuning history that we applied
    > or rejected

![](.//media/image13.png){width="6.5in" height="2.8055555555555554in"}

Once we click on the index we can see a few things

-   Disk space needed

-   The schema, table, and index columns

-   View Script just in case we want to apply recommendation ourselves

-   We can click apply to apply the recommendations

![](.//media/image14.png){width="4.340632108486439in"
height="4.405660542432196in"}

Afterwards the recommendation status pending, it will then become
validated and applied

 

Then we can access the impact afterwards (it will go through
validation)...if there is no

Improvement the change will be reverted.

 

Sometimes the validation step can take a few days since it depends on
the workloads running

against the database.

### Manual Tune Azure SQL DB

 

If application making excessive database reads, it can compromise the DB
performance considering

the network latency.

 

**Tuning Application**

-   In case of several reads, we can try to combine queries into few or
    > one query to get info we need or move to stored procedures.

    -   Can batch ad hoc queries to execute single request to DB

-   Intensive workload - scale up database or scale out. We can also try
    > cross database sharding or functional partitioning

-   Sub-optimal queries - index, tuning execution plan

-   Deadlocking, etc - can use caching mechanism that will reduce
    > roundtrips against database

 

**Tuning Database**

-   Missing indexes or duplicated indexes (must identify queries) and
    > analyze each table to identify index

-   Use query hinting

-   Cross-database sharding - split db into multiple and perform
    > requests against multiple DB

-   Functional partitioning - create instances to duplicate for each of
    > function

    -   Scale independently by function and application

-   Batch queries - avoid multiple request and increase of network
    > latency. Can we batch queries to reduce the number of requests?

+--------------------------------------+------------------------------+
| **Tune your Application**            | **Tune your Database**       |
+======================================+==============================+
| -   Chatty Applications              | -   Enhance database indexes |
+--------------------------------------+------------------------------+
| -   Applications executing intensive | -   Query tuning and hinting |
|     workload against your databases  |                              |
+--------------------------------------+------------------------------+
| -   Applications running sub-optimal | -   Cross-database sharding  |
|     queries                          |                              |
+--------------------------------------+------------------------------+
| -   Applications with data access    | -   Functional partitioning  |
|     concurrency issues, such as      |                              |
|     deadlocking                      |                              |
+--------------------------------------+------------------------------+
|                                      | -   Batch queries            |
+--------------------------------------+------------------------------+
