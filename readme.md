
## Package Dependencies (.NetCore 2.1)

### Basic Requirements


Install-Package Microsoft.AspNetCore.All -Version 2.1.11

Install-Package Microsoft.Extensions.Configuration -Version 2.1.1

Install-Package Microsoft.Extensions.Configuration.Json -Version 2.1.1

Install-Package Microsoft.Extensions.Configuration.Binder -Version 2.1.10

Install-Package Microsoft.EntityFrameworkCore.Tools -Version 2.1.11

Install-Package Microsoft.EntityFrameworkCore -Version 2.1.11

Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.1.11

Install-Package Z.EntityFramework.Plus.EFCore -Version 2.0.7

Install-Package Newtonsoft.Json -Version 12.0.2

Install-Package MailKit -Version 2.1.4

#### 

Install-Package Novell.Directory.Ldap.NETStandard2_0 -Version 3.1.0


### Unit Testing
Install-Package NUnit -Version 3.12.0

Install-Package NUnit3TestAdapter -Version 3.13.0

Install-Package Moq -Version 4.10.1

//Scaffold-DbContext "Server=localhost;Database=Blogging;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

### Microsoft Excel Support

Install-Package DocumentFormat.OpenXml -Version 2.9.1

## appsettings.json sample
```json
    {
	"ConnectionStrings": {
		"DefaultConnection": "data source=localhost;initial catalog=AspNetCoreTemplateDb;integrated security=true;multipleactiveresultsets=True"
    },
    "Logging": {
	    "IncludeScopes": false,
	    "LogLevel": {
		    "Default": "Warning"
	    }
    },
    "security": {
	    "require_windows_authentication": true
    },
    "extendedMailer": {
	    "from": "unit-test@company.com",
	    "isTestMode": true,
	    "skipMailDelivery": false,
	    "replyTo": "noreply@company.com",
	    "testUsers": [
			{ "name": "Yufei Liu", "mail": "feilgly@gmail.com" }
	    ],
	    "alwaysTo": [
		    { "mail": "abc@a.com", "name": "Yufei Liu" }
	    ],
	    "alwaysCc": [
		    { "mail": "abc2@a.com", "name": "Yufei Liu" },
		    { "mail": "abc3@a.com", "name": "Yufei Liu" }
	    ],
	    "alwaysBcc": [
		    { "mail": "abc4@a.com", "name": "Yufei Liu" },
		    { "mail": "abc5@a.com", "name": "Yufei Liu" },
		    { "mail": "abc5@a.com", "name": "Yufei Liu" }
	    ]
	}
  }
```

## Enable Cross Domain with Windows Authentication for Asp.Net Core application

### add the following to the web.config
<system.webServer>
	<security>
		<authorization>
			<add accessType="Deny" users="?" verbs="GET,POST,DELETE,PUT,HEAD,PATCH" />
			<add accessType="Allow" users="*" verbs="OPTIONS" />
		</authorization>
	</security>
	<httpProtocol>
		<customHeaders>
		  <!-- <add name="Access-Control-Allow-Origin" value="*" /> -->
		  <add name="Access-Control-Allow-Methods" value="GET,PUT,POST,DELETE,OPTIONS,HEAD" />
		  <add name="Access-Control-Allow-Headers" value="Content-Type, Accept" />
		</customHeaders>
	</httpProtocol>
</system.webServer>


## Package Dependencies (.Net 4.5)

### For MVC 5

// Update Library Versions to Match SLBMvcSharedModels
Install-Package WebGrease -Version 1.6.0
Install-Package Modernizr 2.8.3
Install-Package Antlr -Version 3.5.0.2
Install-Package Newtonsoft.Json -Version 12.0.1


// Install Mvc 5 Libraries
Install-Package Microsoft.AspNet.Mvc -Version 5.2.7
Install-Package Microsoft.AspNet.WebApi -Version 5.2.7

// Install Entity Framework
Install-Package EntityFramework -Version 6.2.0
//Install-Package EntityFramework.Extended -Version 6.1.0.168
Install-Package Z.EntityFramework.Plus.EF6  -Version 1.10.2

// Install ActionMailerNext
Install-Package ActionMailerNext -Version 3.2.0
Install-Package ActionMailerNext.Mvc5 -Version 3.2.0
Install-Package PreMailer.Net -Version 1.5.4
Install-Package Mandrill -Version 2.4.1

// Install jQuery Libraries
Install-Package jQuery -Version 2.1.4
Install-Package jQuery.Validation -Version 1.13.1

// Install MvcFlash
Install-Package MvcFlash.Core -Version 2.1.0
Install-Package MvcFlash.Web -Version 2.0.1

// Install PageList
Install-Package PagedList -Version 1.17.0.0
Install-Package PagedList.Mvc -Version 4.5.0.0

// Install Glimpse
Install-Package Glimpse.MVC5 -Version 1.5.3
Install-Package Glimpse.AspNet -Version 1.9.2
Install-Package Glimpse.EF6 -Version 1.6.5

// Optional - Install Rotativa - PDF generator
Install-Package Rotativa -Version 1.6.4


### Optional - you may need it
Install-Package Microsoft.Data.Edm -Version 5.7.0
Install-Package Microsoft.Data.OData -Version 5.7.0
Install-Package Microsoft.AspNet.WebApi.OData -Version 5.7.0
Install-Package System.Spatial -Version 5.7.0
Install-Package Flurl -Version 2.21
Install-Package Flurl.Http -Version 1.0.2


### Remove the following package (Optional)
Uninstall-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform
Uninstall-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform

### Microsoft Excel Support
Install-Package DocumentFormat.OpenXml -Version 2.9.1

### NUnit Test Framework, only on Tests projects

// remember to install NUnitTestAdapter extension in VS
Install-Package NUnit -Version 3.12.0
Install-Package NUnit3TestAdapter -Version 3.13.0
//Install-Package NUnit.Runners -Version 3.2.1
Install-Package Moq -Version 4.10.1

### for Console App
Install-Package RazorEngine -Version 3.10.0
Install-Package Microsoft.AspNet.Razor -Version 3.2.7
Install-Package ActionMailerNext -Version 3.2.0
//Install-Package ActionMailerNext.Standalone -Version 3.2.0

