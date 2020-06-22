![Microsoft Cloud Workshops](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
Migrate EDW to Azure SQL Data Warehouse
</div>

<div class="MCWHeader2">
Before the hands-on lab setup guide
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

- [Migrate EDW to Azure SQL Data Warehouse before the hands-on lab set up guide](#migrate-edw-to-azure-sql-data-warehouse-before-the-hands-on-lab-set-up-guide)
  - [Requirements](#requirements)
  - [Before the hands-on lab](#before-the-hands-on-lab)
    - [Task 1: Deploy the source environment](#task-1-deploy-the-source-environment)

<!-- /TOC -->

# Migrate EDW to Azure SQL Data Warehouse before the hands-on lab set up guide

## Requirements

1.  Microsoft Azure subscription

## Before the hands-on lab

In this exercise, you will deploy the source environment for this lab. The source environment is designed to represent the existing on-premises environment you will migrate to Azure SQL Data Warehouse.

### Task 1: Deploy the source environment

1.  Browse to the Azure Portal at <https://portal.azure.com> and verify that you are logged in with the subscription that you wish to use for this lab.

2.  Select the **Deploy to Azure** button below. 

    <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FMicrosoft%2FMCW-Migrate-EDW-to-Azure-SQL-Data-Warehouse%2Fmaster%2FHands-on%20lab%2Flabfiles%2Fazure-deploy.json" rel="nofollow">
    <img src="https://camo.githubusercontent.com/9285dd3998997a0835869065bb15e5d500475034/687474703a2f2f617a7572656465706c6f792e6e65742f6465706c6f79627574746f6e2e706e67" data-canonical-src="http://azuredeploy.net/deploybutton.png" style="max-width:100%;">
    </a>

3. This will launch a template deployment in the Azure Portal. Under the Resource Group parameter select **Create New** and type **CohoOnPremEnvironment** and select **OK**.

    ![Screenshot of the Azure Custom deployment blade, next to Resouce Group, the Create new link is highlighted and in the resulting new resource group window OnPremEnvironment is typed into the name field.](images/Hands-onlabstep-by-step-MigrateEDWtoAzureSQLDataWarehouseimages/media/2019-01-22-19-25-46.png "Custom deployment blade")

4. Set the **Location** and the **Region** to a region close to you, accept the terms and conditions at the bottom, then select **Purchase**.

5. The deployment will take about 15-20 minutes.

You should follow all steps provided *before* performing the Hands-on lab.
