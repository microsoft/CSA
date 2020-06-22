Tutorial: Migrate SQL Server to Azure SQL Database offline using DMS
====================================================================

You can use Azure Database Migration Service to migrate the databases
from a SQL Server instance to [Azure SQL
Database](https://docs.microsoft.com/azure/sql-database/).

In this tutorial, you migrate the **Adventureworks2012** database
restored to an on-premises instance of SQL Server 2016 (or later) to a
single database or pooled database in Azure SQL Database by using Azure
Database Migration Service.

In this tutorial, you learn how to:

-   Assess your on-premises database by using the Data Migration
    Assistant.

-   Migrate the sample schema by using the Data Migration Assistant.

-   Create an instance of Azure Database Migration Service.

-   Create a migration project by using Azure Database Migration
    Service.

-   Run the migration.

-   Monitor the migration.

-   Download a migration report.

This article describes an offline migration from SQL Server to a single
database or pooled database in Azure SQL Database. For an online
migration, see [Migrate SQL Server to Azure SQL Database online using
DMS](https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/dms/tutorial-sql-server-azure-sql-online.md).

Prerequisites
-------------

To complete this tutorial, you need to:

-   Download and install [SQL Server 2016 or
    later](https://www.microsoft.com/sql-server/sql-server-downloads).

-   Enable the TCP/IP protocol, which is disabled by default during SQL
    Server Express installation, by following the instructions in the
    article [Enable or Disable a Server Network
    Protocol](https://docs.microsoft.com/sql/database-engine/configure-windows/enable-or-disable-a-server-network-protocol#SSMSProcedure).

-   Create a single (or pooled) database in Azure SQL Database, which
    you do by following the detail in the article [Create a single
    database in Azure SQL Database using the Azure
    portal](https://docs.microsoft.com/azure/sql-database/sql-database-single-database-get-started).

> \[NOTE\] If you use SQL Server Integration Services (SSIS) and want
> to migrate the catalog database for your SSIS projects/packages
> (SSISDB) from SQL Server to Azure SQL Database, the destination SSISDB
> will be created and managed automatically on your behalf when you
> provision SSIS in Azure Data Factory (ADF). For more information about
> migrating SSIS packages, see the article [Migrate SQL Server
> Integration Services packages to
> Azure](https://docs.microsoft.com/azure/dms/how-to-migrate-ssis-packages).

-   Download and install the [Data Migration
    Assistant](https://www.microsoft.com/download/details.aspx?id=53595) v3.3
    or later.

-   Create a Microsoft Azure Virtual Network for Azure Database
    Migration Service by using the Azure Resource Manager deployment
    model, which provides site-to-site connectivity to your on-premises
    source servers by using
    either [ExpressRoute](https://docs.microsoft.com/azure/expressroute/expressroute-introduction) or [VPN](https://docs.microsoft.com/azure/vpn-gateway/vpn-gateway-about-vpngateways).
    For more information about creating a virtual network, see
    the [Virtual Network
    Documentation](https://docs.microsoft.com/azure/virtual-network/),
    and especially the quickstart articles with step-by-step details.

> \[!NOTE\] During virtual network setup, if you use ExpressRoute with
> network peering to Microsoft, add the following
> service [endpoints](https://docs.microsoft.com/azure/virtual-network/virtual-network-service-endpoints-overview) to
> the subnet in which the service will be provisioned:

-   Target database endpoint (for example, SQL endpoint, Cosmos DB
    endpoint, and so on)

-   Storage endpoint

-   Service bus endpoint

> This configuration is necessary because Azure Database Migration
> Service lacks internet connectivity.
>
> If you don't have site-to-site connectivity between the on-premises
> network and Azure or if there is limited site-to-site connectivity
> bandwidth, consider using Azure Database Migration Service in hybrid
> mode (Preview). Hybrid mode leverages an on-premises migration worker
> together with an instance of Azure Database Migration Service running
> in the cloud. To create an instance of Azure Database Migration
> Service in hybrid mode, see the article [Create an instance of Azure
> Database Migration Service in hybrid mode using the Azure
> portal](https://aka.ms/dms-hybrid-create).

-   Ensure that your virtual network Network Security Group rules don\'t
    block the following inbound communication ports to Azure Database
    Migration Service: 443, 53, 9354, 445, 12000. For more detail on
    Azure virtual network NSG traffic filtering, see the article [Filter
    network traffic with network security
    groups](https://docs.microsoft.com/azure/virtual-network/virtual-networks-nsg).

-   Configure your [Windows Firewall for database engine
    access](https://docs.microsoft.com/sql/database-engine/configure-windows/configure-a-windows-firewall-for-database-engine-access).

-   Open your Windows firewall to allow Azure Database Migration Service
    to access the source SQL Server, which by default is TCP port 1433.

-   If you\'re running multiple named SQL Server instances using dynamic
    ports, you may wish to enable the SQL Browser Service and allow
    access to UDP port 1434 through your firewalls so that Azure
    Database Migration Service can connect to a named instance on your
    source server.

-   When using a firewall appliance in front of your source database(s),
    you may need to add firewall rules to allow Azure Database Migration
    Service to access the source database(s) for migration.

-   Create a server-level IP [firewall
    rule](https://docs.microsoft.com/azure/sql-database/sql-database-firewall-configure) for
    Azure SQL Database to allow Azure Database Migration Service access
    to the target databases. Provide the subnet range of the virtual
    network used for Azure Database Migration Service.

-   Ensure that the credentials used to connect to source SQL Server
    instance have [CONTROL
    SERVER](https://docs.microsoft.com/sql/t-sql/statements/grant-server-permissions-transact-sql) permissions.

-   Ensure that the credentials used to connect to target Azure SQL
    Database instance have CONTROL DATABASE permission on the target
    Azure SQL databases.

Assess your on-premises database
--------------------------------

Before you can migrate data from a SQL Server instance to a single
database or pooled database in Azure SQL Database, you need to assess
the SQL Server database for any blocking issues that might prevent
migration. Using the Data Migration Assistant v3.3 or later, follow the
steps described in the article [Performing a SQL Server migration
assessment](https://docs.microsoft.com/sql/dma/dma-assesssqlonprem) to
complete the on-premises database assessment. A summary of the required
steps follows:

1.  In the Data Migration Assistant, select the New (+) icon, and then
    select the **Assessment** project type.

2.  Specify a project name, in the **Source server type** text box,
    select **SQL Server**, in the **Target server type** text box,
    select **Azure SQL Database**, and then select **Create** to create
    the project.

> When you\'re assessing the source SQL Server database migrating to a
> single database or pooled database in Azure SQL Database, you can
> choose one or both of the following assessment report types:

-   Check database compatibility

-   Check feature parity

> Both report types are selected by default.

3.  In the Data Migration Assistant, on the **Options** screen,
    select **Next**.

4.  On the **Select sources** screen, in the **Connect to a
    server** dialog box, provide the connection details to your SQL
    Server, and then select **Connect**.

5.  In the **Add sources** dialog box, select **AdventureWorks2012**,
    select **Add**, and then select **Start Assessment**.

> \[!NOTE\] If you use SSIS, DMA does not currently support the
> assessment of the source SSISDB. However, SSIS projects/packages will
> be assessed/validated as they are redeployed to the destination SSISDB
> hosted by Azure SQL Database. For more information about migrating
> SSIS packages, see the article [Migrate SQL Server Integration
> Services packages to
> Azure](https://docs.microsoft.com/azure/dms/how-to-migrate-ssis-packages).
>
> When the assessment is complete, the results display as shown in the
> following graphic:
>
> ![Assess data migration](.//media/image1.png){width="6.5in"
> height="3.279861111111111in"}
>
> For single databases or pooled databases in Azure SQL Database, the
> assessments identify feature parity issues and migration blocking
> issues for deploying to a single database or pooled database.

-   The **SQL Server feature parity** category provides a comprehensive
    set of recommendations, alternative approaches available in Azure,
    and mitigating steps to help you plan the effort into your migration
    projects.

-   The **Compatibility issues** category identifies partially supported
    or unsupported features that reflect compatibility issues that might
    block migrating SQL Server database(s) to Azure SQL Database.
    Recommendations are also provided to help you address those issues.

6.  Review the assessment results for migration blocking issues and
    feature parity issues by selecting the specific options.

Migrate the sample schema
-------------------------

After you\'re comfortable with the assessment and satisfied that the
selected database is a viable candidate for migration to a single
database or pooled database in Azure SQL Database, use DMA to migrate
the schema to Azure SQL Database.

\[!NOTE\] Before you create a migration project in Data Migration
Assistant, be sure that you have already provisioned an Azure SQL
database as mentioned in the prerequisites. For purposes of this
tutorial, the name of the Azure SQL Database is assumed to
be **AdventureWorksAzure**, but you can provide whatever name you wish.

\[!IMPORTANT\] If you use SSIS, DMA does not currently support the
migration of source SSISDB, but you can redeploy your SSIS
projects/packages to the destination SSISDB hosted by Azure SQL
Database. For more information about migrating SSIS packages, see the
article [Migrate SQL Server Integration Services packages to
Azure](https://docs.microsoft.com/azure/dms/how-to-migrate-ssis-packages).

To migrate the **AdventureWorks2012** schema to a single database or
pooled database Azure SQL Database, perform the following steps:

1.  In the Data Migration Assistant, select the New (+) icon, and then
    under **Project type**, select **Migration**.

2.  Specify a project name, in the **Source server type** text box,
    select **SQL Server**, and then in the **Target server type** text
    box, select **Azure SQL Database**.

3.  Under **Migration Scope**, select **Schema only**.

> After performing the previous steps, the Data Migration Assistant
> interface should appear as shown in the following graphic:
>
> ![Create Data Migration Assistant
> Project](.//media/image2.png){width="6.5in"
> height="3.6868055555555554in"}

4.  Select **Create** to create the project.

5.  In the Data Migration Assistant, specify the source connection
    details for your SQL Server, select **Connect**, and then select
    the **AdventureWorks2012** database.

> ![Data Migration Assistant Source Connection
> Details](.//media/image3.png){width="6.5in"
> height="3.6868055555555554in"}

6.  Select **Next**, under **Connect to target server**, specify the
    target connection details for the Azure SQL Database,
    select **Connect**, and then select
    the **AdventureWorksAzure** database you had pre-provisioned in
    Azure SQL Database.

> ![Data Migration Assistant Target Connection
> Details](.//media/image4.png){width="6.5in"
> height="4.748611111111111in"}

7.  Select **Next** to advance to the **Select objects** screen, on
    which you can specify the schema objects in
    the **AdventureWorks2012** database that need to be deployed to
    Azure SQL Database.

> By default, all objects are selected.
>
> ![Generate SQL Scripts](.//media/image5.png){width="6.5in"
> height="4.748611111111111in"}

8.  Select **Generate SQL script** to create the SQL scripts, and then
    review the scripts for any errors.

> ![Schema Script](.//media/image6.png){width="6.5in"
> height="4.748611111111111in"}

9.  Select **Deploy schema** to deploy the schema to Azure SQL Database,
    and then after the schema is deployed, check the target server for
    any anomalies.

> ![Deploy Schema](.//media/image7.png){width="6.5in"
> height="4.748611111111111in"}

Register the Microsoft.DataMigration resource provider
------------------------------------------------------

1.  Sign in to the Azure portal. Search for and
    select **Subscriptions**.

> ![Show portal subscriptions](.//media/image8.png){width="6.5in"
> height="2.7868055555555555in"}

2.  Select the subscription in which you want to create the instance of
    Azure Database Migration Service, and then select **Resource
    providers**.

> ![Show resource providers](.//media/image9.png){width="6.5in"
> height="4.977777777777778in"}

3.  Search for migration, and then
    select **Register** for **Microsoft.DataMigration**.

> ![Register resource provider](.//media/image10.png){width="6.5in"
> height="3.402083333333333in"}

Create an instance
------------------

1.  In the Azure portal menu or on the **Home** page, select **Create a
    resource**. Search for and select **Azure Database Migration
    Service**.

> ![Azure Marketplace](.//media/image11.png){width="6.5in"
> height="2.401388888888889in"}

2.  On the **Azure Database Migration Service** screen,
    select **Create**.

> ![Create Azure Database Migration Service
> instance](.//media/image12.png){width="6.5in"
> height="2.9756944444444446in"}

3.  On the **Create Migration Service** screen, specify a name for the
    service, the subscription, and a new or existing resource group.

4.  Select the location in which you want to create the instance of
    Azure Database Migration Service.

5.  Select an existing virtual network or create a new one.

> The virtual network provides Azure Database Migration Service with
> access to the source SQL Server and the target Azure SQL Database
> instance.
>
> For more information about how to create a virtual network in the
> Azure portal, see the article [Create a virtual network using the
> Azure portal](https://aka.ms/DMSVnet).

6.  Select a pricing tier.

> For more information on costs and pricing tiers, see the [pricing
> page](https://aka.ms/dms-pricing).
>
> ![Configure Azure Database Migration Service instance
> settings](.//media/image13.png){width="3.1555555555555554in"
> height="9.0in"}

7.  Select **Create** to create the service.

Create a migration project
--------------------------

After the service is created, locate it within the Azure portal, open
it, and then create a new migration project.

1.  In the Azure portal menu, select **All services**. Search for and
    select **Azure Database Migration Services**.

> ![Locate all instances of Azure Database Migration
> Service](.//media/image14.png){width="6.5in"
> height="3.5381944444444446in"}

2.  On the **Azure Database Migration Services** screen, select the
    Azure Database Migration Service instance that you created.

3.  Select **New Migration Project**.

> ![Locate your instance of Azure Database Migration
> Service](.//media/image15.png){width="6.5in"
> height="2.7423611111111112in"}

4.  On the **New migration project** screen, specify a name for the
    project, in the **Source server type** text box, select **SQL
    Server**, in the **Target server type** text box, select **Azure SQL
    Database**, and then for **Choose type of activity**,
    select **Offline data migration**.

> ![Create Database Migration Service
> Project](.//media/image16.png){width="3.136111111111111in"
> height="9.0in"}

5.  Select **Create and run activity** to create the project and run the
    migration activity.

Specify source details
----------------------

1.  On the **Migration source detail** screen, specify the connection
    details for the source SQL Server instance.

> Make sure to use a Fully Qualified Domain Name (FQDN) for the source
> SQL Server instance name. You can also use the IP Address for
> situations in which DNS name resolution isn\'t possible.

2.  If you have not installed a trusted certificate on your source
    server, select the **Trust server certificate** check box.

> When a trusted certificate is not installed, SQL Server generates a
> self-signed certificate when the instance is started. This certificate
> is used to encrypt the credentials for client connections.
>
> \[!CAUTION\] TLS connections that are encrypted using a self-signed
> certificate do not provide strong security. They are susceptible to
> man-in-the-middle attacks. You should not rely on TLS using
> self-signed certificates in a production environment or on servers
> that are connected to the internet.
>
> ![Source Details](.//media/image17.png){width="6.284027777777778in"
> height="9.0in"}
>
> \[!IMPORTANT\] If you use SSIS, DMS does not currently support the
> migration of source SSISDB, but you can redeploy your SSIS
> projects/packages to the destination SSISDB hosted by Azure SQL
> Database. For more information about migrating SSIS packages, see the
> article [Migrate SQL Server Integration Services packages to
> Azure](https://docs.microsoft.com/azure/dms/how-to-migrate-ssis-packages).

Specify target details
----------------------

1.  Select **Save**, and then on the **Migration target
    details** screen, specify the connection details for the target
    Azure SQL Database, which is the pre-provisioned Azure SQL Database
    to which the **AdventureWorks2012** schema was deployed by using the
    Data Migration Assistant.

> ![Select Target](.//media/image18.png){width="6.274305555555555in"
> height="9.0in"}

2.  Select **Save**, and then on the **Map to target databases** screen,
    map the source and the target database for migration.

> If the target database contains the same database name as the source
> database, Azure Database Migration Service selects the target database
> by default.
>
> ![Map to target databases](.//media/image19.png){width="6.5in"
> height="4.993055555555555in"}

3.  Select **Save**, on the **Select tables** screen, expand the table
    listing, and then review the list of affected fields.

> Azure Database Migration Service auto selects all the empty source
> tables that exist on the target Azure SQL Database instance. If you
> want to remigrate tables that already include data, you need to
> explicitly select the tables on this blade.
>
> ![Select tables](.//media/image20.png){width="6.5in"
> height="4.982638888888889in"}

4.  Select **Save**, on the **Migration summary** screen, in
    the **Activity name** text box, specify a name for the migration
    activity.

5.  Expand the **Validation option** section to display the **Choose
    validation option** screen, and then specify whether to validate the
    migrated databases for **Schema comparison**, **Data consistency**,
    and **Query correctness**.

> ![Choose validation option](.//media/image21.png){width="6.5in"
> height="6.215972222222222in"}

6.  Select **Save**, review the summary to ensure that the source and
    target details match what you previously specified.

> ![Migration Summary](.//media/image22.png){width="6.383333333333334in"
> height="9.0in"}

Run the migration
-----------------

-   Select **Run migration**.

> The migration activity window appears, and the **Status** of the
> activity is **Pending**.
>
> ![Activity Status](.//media/image23.png){width="6.5in"
> height="4.094444444444444in"}

Monitor the migration
---------------------

1.  On the migration activity screen, select **Refresh** to update the
    display until the **Status** of the migration shows
    as **Completed**.

> ![Activity Status Completed](.//media/image24.png){width="6.5in"
> height="4.2375in"}

2.  After the migration completes, select **Download report** to get a
    report listing the details associated with the migration process.

3.  Verify the target database(s) on the target Azure SQL Database.

### Additional resources

-   [SQL migration using Azure Data Migration
    Service](https://www.microsoft.com/handsonlabs/SelfPacedLabs/?storyGuid=3b671509-c3cd-4495-8e8f-354acfa09587) hands-on
    lab.

-   For information about known issues and limitations when performing
    online migrations to Azure SQL Database, see the article [Known
    issues and workarounds with Azure SQL Database online
    migrations](https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/dms/known-issues-azure-sql-online.md).

-   For information about Azure Database Migration Service, see the
    article [What is Azure Database Migration
    Service?](https://docs.microsoft.com/azure/dms/dms-overview).

-   For information about Azure SQL Database, see the article [What is
    the Azure SQL Database
    service?](https://docs.microsoft.com/azure/sql-database/sql-database-technical-overview).
