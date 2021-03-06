# BannersRest.API
A minimalistic **REST API** to manage Html Banners on built on top **.Net Core** and **mongoDb** database.


## Solution overview 

the solution contains two test project:
 - the Main project **BannerFlow.Rest** 
 - A __UnitTest__ project that tests the behaviour of the REST ``Controller`` with a mock of the db service, and unit tests for  `HtmlUtility` which provides Html correctness checks.
 - An __IntegartionTest__ project, a WebClient that make calls to the different REST endpoints and check the whole flow.
 
## Testing the project 

- Make sure MongoDb is installed on your computer
- Run the **BannerFlow.Rest** project using VisualStudio
- Import ``Banner.Rest.API.postman_Req_collection.json`` file in [Postman](https://www.getpostman.com/).
    > It contains a collection of Http requests to interact with the REST API 
   ### Alternatively
- I created **BannersRest.Client** (AngularJs WebApp) to interact with Rest API. It provides very simple UI's to create, list, update and delete Banners.
- You can clone the app from here https://github.com/bouguila/BannersRest.Client and run it to try different CRUD operations
  > The [Readme](https://github.com/bouguila/BannersRest.Client#to-run-the-app) of the app provides more information on how to run it

## API Specification:

#### Authorization

- Only requests with the headers containing **`x-api-key`**:`abcde1234` are allowed. 
  > (wanted to keep it simple as we have no concept of users and claims for the moment, and of course the key in clear can't be added to README file on real project :) )
- to remove this request restriction, please comment the `ApplyAuthorization();` in ``Startup.cs`` file

### Endpoints

- GET resources:

   - ``GET`` **api/banner**    :   &nbsp;&nbsp; Get all banners
   - ``GET`` **api/banner/{banner_id}** :  &nbsp;&nbsp;Get specific banner 
   - ``GET`` **api/banner/{banner_id}/html** :   &nbsp;&nbsp;Return the html content of a specific banner 


- POST resources:

   - ``POST`` **api/banner**    :   &nbsp;&nbsp;Create new Banner.

     - **Request Headers**
        - **`x-api-key`**:`abcde1234`
        - **content-type** : application/json
     
     - **Request Body**

    <pre>
    {
        <b> Id </b>: &nbsp;&nbsp;int, required
        <b> Html </b>: &nbsp;&nbsp;string, should contain valid html code
        <b> Created </b>: &nbsp;&nbsp;DatetTime, optional, default is the timestamp of the object creation
        <b> Modified </b>: &nbsp;&nbsp;Datetime, optional, default is null
    }
    </pre>


- PUT resources:

  - ``PUT`` **api/banner/{banner_id}** :   &nbsp;&nbsp;Update a specific banner 

     - **Request Headers**
        - **`x-api-key`**:`abcde1234`
        - content-type : application/json
        
     - **Request Body**

    <pre>
    {
        <b> Id </b>: &nbsp;&nbsp;int, required, should be the same as request path
        <b> Html </b>: &nbsp;&nbsp;string, should contain valid html code
        <b> Created </b>: &nbsp;&nbsp;DatetTime, optional, default is the timestamp of the object creation
        <b> Modified </b>: &nbsp;&nbsp;Datetime, optional, default is null
    }
    </pre>


- DELETE resources:

  - ``DELETE`` **api/banner/{banner_id}** :&nbsp;&nbsp;Delete a specific banner
