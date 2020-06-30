# Create and Deploy CLR to SQL MI #
05/14/2020<br>
Carl Neal - Senior Cloud Solution Architect at Microsoft


### Steps to Deploying a CLR Assembly to SQL MI ###

1.  Create the SQL MI Instance<br>
    a.  I enabled Public endpoint so I can work on this locally for my computer

    b.  Go to the NSG and create an Inbound firewall rule to allow your Personal IP Address to Access on port 3342 (This allows remote connections from SSMS)

2.  Connect to the SQL MI Instance from SSMS v18.5<br>
    a.  Connection should look like
        > \"sqlmi-samplename.public.f2a1e029dfs8251.database.windows.net,3342\"
        > (make sure to add the ,3342 )

3.  Enable clr and strict security<br>
    EXEC sp\_configure \'show advanced options\', 1;
    RECONFIGURE;

    sp\_configure \'clr enabled\', 1
    RECONFIGURE
    
    sp\_configure \'clr strict security\', 0
    RECONFIGURE
 
4.  Confirm the SQL MI .Net Framework Runtime Version<br>
    a.  SELECT olm.\[name\], olm.\[file\_version\]\
        > FROM sys.dm\_os\_loaded\_modules olm\
        > WHERE olm.\[name\] LIKE N\'%mscoreei.dll%\';
    ![](.//media/image1.png)
5.  Create the database in SQL MI that you plan on deploying the CLR too

6.  Create a Visual Studio SQL Server Database Project

7.  Add a new SQL CLR C\# item by Right Clicking VS Project -\> Add -\> New Item -\> SQL CLR C\# (or you can go to Project -\> Add New -\> Item from task bar)<br>
    ![](.//media/image2.png)

8.  Write the appropriate code for your CLR
    ![](.//media/image3.png)

9.  Ensure that the target server SQL Server version matches the > database created above (Right Click Project -\> Properties -\> Project Settings)
    ![](.//media/image4.png)

10. Confirm that the .NET Framework Version matches that of the SQL MI > Instance
    ![](.//media/image5.png)

11. Publish the CLR to the SQL MI Instance (Right click -\> Publish)<br>
    a.  Add the credentials to the Target Database connection and test > connection to confirm access (add the ,3342 to the connection > string)

    ![](.//media/image6.png)

12. Confirm that the CLR Assembly was appropriately added to the SQL MI Instance
    ![](.//media/image7.png)


###To Deploy a CLR Assembly with External\_Access###

1.  Make the database set to be Trustworthy in VS <br>
    a.  RC Project -\> Properties -\> Project Settings -\> Database
        > Settings -\> Misc Tab -\> Check Trustworthy
    ![](.//media/image8.png)

2.  Make Database Trustworthy in SQL MI<br>
    a.  ALTER DATABASE \[SQLMICLRDemo\] SET TRUSTWORTHY ON
