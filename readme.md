# Wonen In Zwaluwpark #
This is a project I've done for our new house in Arnhem, The Netherlands. It's purpose was to connect to the people and show progress of the development.

It is build with MVC3 and currently hosted on AppHarbor (http://woneninzwaluwpark.apphb.com)

# Features #

- 90+ score on pagespeed, the only real optimization was to create css per page
- OpenID, LiveID, Facebook login (LiveID and Facebook build from scratch)
- YUI Compression on build

# YUI Compression #

On postbuild I do the following to create a minimal site.css

    PATH=%PATH%;C:\Program Files\Java\jre7\bin
    SET CLASSPATH=$(ProjectDir)Tools
    CD $(ProjectDir)
    java -jar Tools\yuicompressor-2.4.6.jar Style\Site.css -o Style\Site.min.css

In the _layout.cshtml, I use the minimal only on the production machine by checking the Request.IsLocal

    @if (Request.IsLocal) {
       <link href="@Url.Content("~/Style/Site.css")" rel="stylesheet" type="text/css" />
    } else {
        <link href="@Url.Content("~/Style/Site.min.css")" rel="stylesheet" type="text/css" />
    }