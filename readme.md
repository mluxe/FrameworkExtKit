
## Package Dependencies (.NetCore 2.1)

### Basic Requirements

Install-Package Microsoft.EntityFrameworkCore.Tools -Version 2.1.11

Install-Package Microsoft.EntityFrameworkCore -Version 2.1.11

Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.1.11

Install-Package Z.EntityFramework.Plus.EFCore -Version 2.0.7

Install-Package Newtonsoft.Json -Version 12.0.2

#### 

Install-Package Novell.Directory.Ldap.NETStandard2_0 -Version 3.1.0


### Unit Testing
Install-Package NUnit -Version 3.12.0

Install-Package NUnit3TestAdapter -Version 3.13.0

Install-Package Moq -Version 4.10.1

### Microsoft Excel Support

Install-Package DocumentFormat.OpenXml -Version 2.9.1

## appsettings.json sample
```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "data source=(localdb)\\MSSQLLocalDB;initial catalog=FrameworkExtKit.NetCore.Tests;integrated security=true;multipleactiveresultsets=True"
      },
      "DirectoryAccountService": {
        "ClassName": "FrameworkExtKit.Services.DirectoryServices.LdapDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.UnitTest.Services.DirectoryService.MemoryDirectoryAccountService, FrameworkExtKit.UnitTest"
      },
      "LdapSettings": {
        "ServerName": "ldap.company.com",
        "ServerPort": 389,
        "UseSSL": false,
        "Credentials": {
          "DomainUserName": "",
          "Password": ""
        },
        "SearchBase": "o=company,c=an",
        "ContainerName": "",
        "DomainName": "company.com",
        "DomainDistinguishedName": "o=company,c=an"
      }
    }
```

## Package Dependencies (.Net 4.5)

### For MVC 5

Install-Package Newtonsoft.Json -Version 12.0.1

// Install Entity Framework
Install-Package EntityFramework -Version 6.2.0
Install-Package Z.EntityFramework.Plus.EF6  -Version 1.10.2

// Optional - Install Rotativa - PDF generator
Install-Package Rotativa -Version 1.6.4

### Microsoft Excel Support
Install-Package DocumentFormat.OpenXml -Version 2.9.1

### NUnit Test Framework, only on Tests projects

// remember to install NUnitTestAdapter extension in VS
Install-Package NUnit -Version 3.12.0
Install-Package NUnit3TestAdapter -Version 3.13.0
Install-Package Moq -Version 4.10.1

## Web.Config
```xml
  <connectionStrings>
    <add name="DBDirectoryService" connectionString="data source=(localdb)\MSSQLLocalDB;integrated security=true;initial catalog=DBDirectoryService_Public;" providerName="System.Data.SqlClient" />
  </connectionStrings>
  <appSettings>
    <!-- User Directory Service-->
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.LdapDirectoryAccountService" />
    <!--
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.ActiveDirectoryAccountService" />
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.DB.DbDirectoryAccountService" />
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.MemoryDirectoryAccountService, FrameworkExtKit.UnitTest" />
    -->
    <add key="DirectoryAccountService.RootEntryPath" value="LDAP://ldap.company.com/o=company,c=an" />
    <!--<add key="DirectoryAccountService.RootEntryPath" value="DB://DBDirectoryService_Public" />-->
  </appSettings>
```
