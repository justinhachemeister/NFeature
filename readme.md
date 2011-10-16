NFeature
====

A simple feature configuration system. 

Feature configuration walls enable you to integrate your code earlier, which brings lots of goodness (such as helping to avoid branch merge problems.)

**NOTE: this is pre-release quality software. There will be bugs/inaccuracies in the documentation. If you find an issue, please help me by letting me know at ```ben@bj.ma```.**

How to use:
--------
**1. Define some features and a tenant**

Please note that requirement to specify a tenant type may eventually be removed.	
	
In your code:

```C#

	
	public enum Feature
	{
		MyFeature,
		MyOtherFeature,
		MyOtherOtherFeature,
	}
		
	public enum Tenant
	{
		All, 
	}

```

In your configuration:

Please note that the logic to determine whether a feature is available is specified in the ```IFeatureManifestCreationStrategy``` instance you inject into the ```FeatureManifestService``` and (optionally, depending on your implementation of the aforementioned strategy) by the availability-checking function you inject into the ```FeatureSettingAvailabilityChecker```. 

Two concrete implementations of ```IFeatureManifestCreationStrategy``` are provided of-the-box: ```ManifestCreationStrategyDefault``` and ```ManifestCreationStrategyConsideringStateCookieTenantAndTime```. A single default availability checker function is provided out of the box ```DefaultFunctions.AvailabilityCheckFunction```, which may be used when the feature state, tenant, feature visibility mode and system time are known.

```XML

	
    <features>
		<add name="MyFeature" state="Enabled" /> <!-- will be available to all -->
		<add name="MyOtherFeature" state="Previewable" /> <!-- will only be available to users who meet the feature-preview criteria* -->
		<add name="MyOtherOtherFeature" state="Disabled" /> <!-- not available -->
	</features>
	
```

**2. Define the availability-checking function**


```C#


	//Here is a function that will only return 'true' if the feature is TestFeatureA
	//Your function might be more elaborate involving, for example, checking of site 
	//load or user role. 
	Func<FeatureSetting<Feature, Tenant>, EmptyArgs, bool> fn = (f, args) => f == Feature.TestFeatureA; 

```

**3. Take care of feature manifest initialization**

For a working example of this see the integration test named ```FeatureEnumExtensionsTests``` in the ```NFeature.Test.Slow``` project, within the main solution.

```C#


	//NOTE: I suggest hiding this ugly initialization logic away in the IOC container configuration	
	var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature, Tenant>();
	var availabilityChecker = new FeatureSettingAvailabilityChecker<Feature, Tenant>(fn); //from step 2      
	var featureSettingService = new FeatureSettingService<Feature, Tenant, EmptyArgs>(availabilityChecker, featureSettingRepo);
	var manifestCreationStrategy = new ManifestCreationStrategyDefault(featureSettingRepo, featureSettingService); //we use the default for this example
	var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
	var featureManifest = featureManifestService.GetManifest();	


```

**4. Add code that is conditional on feature availability**
	
```C#


	if(Feature.MyFeature.IsAvailable(featureManifest)) //featureManifest ideally supplied via IOC container
	{
		//do some cool stuff
	}
	
```

**5. Configure any feature dependencies**

```XML

	
    <features>
		<add name="MyFeature" dependencies="MyOtherFeature,MyOtherOtherFeature" />
	</features>

```

**6. Optionally configure feature settings using Json (neatly side-stepping the Microsoft XML configuration functionality)**
	
```XML

	
	<features>
		<add name="MyFeature" settings="{ mySetting:'mySettingValue', 
				   	                myOtherSetting:'myOtherSettingValue' }" />
	</features>

```

**7. Optionally specify dates for feature availability**

```XML

	
    <features>
		<add name="MyFeature" startDtg="23/03/2012:00:00:00" /> <!-- available from 23rd March 2012 forever -->
		<add name="MyOtherFeature" startDtg="23/03/2012:00:00:00" endDtg="24/03/2012:00:00:00" /> <!-- available from 23rd March 2012 until the 24th -->
		<add name="MyOtherOtherFeature" endDtg="24/03/2012:00:00:00" /> <!-- available until 24th March 2012 -->
	</features>

```

**8. Optionally mark your feature as ```Established``` to indicate that it is now integral to your application**

```XML

	
	<features>
		<add name="MyFeature" state="Established" />
	</features>

```

**9. ...**

**10. Profit!**


How to build and/or run the tests:
--------

1. Run `/build/build.bat`
1. Type in the desired option
1. Hit return