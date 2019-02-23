# BannersRest.API
A minimalistic **REST API** to manage Html Banners on built on top **.Net Core** and **mongoDb** database.


## Solution overview 

the solution contains two test project:
 - the Main project **BannerFlow.Rest** 
 - A __UnitTest__ project that tests the behaviour of the REsT ``Controller`` with a mock of the db service, and unit tests for  `HtmlUtility` which provides Html correctness checks.
 - An __IntegartionTest__ project, a WebClient that make calls to the different REST endpoints and check the whole flow.
 
## Testing the project 

- Make sure MongoDb is installed on your computer
- Run the **BannerFlow.Rest** project using VisualStudio
- Import ``Banner.Rest.API.postman_Req_collection.json`` file in [Postman](https://www.getpostman.com/).
 > It contains a collection of Http requests to interact with the REsT API 


## API Specification:

#### Authorization

- Only requests with the headers containing **`x-api-key`**:`abcde1234` are allowed. 
  > (wanted to keep it simple as we have no concept of users and claims for the moment)

### Endpoints

- GET resources:

   - ``GET`` **api/banner**    :   &nbsp;&nbsp; get all banners
   - ``GET`` **api/banner/{banner_id}** :  &nbsp;&nbsp;get specific banner 
   - ``GET`` **api/banner/{banner_id}/html** :   &nbsp;&nbsp;returns the html content of a specific banner 


- POST resources:

   - ``POST`` **api/banner**    :   &nbsp;&nbsp;get all banners.

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

  - ``PUT`` **api/banner/{banner_id}** :   &nbsp;&nbsp;Updates a specific banner 

     - **Request Body**

    <pre>
    {
        <b> Id </b>: &nbsp;&nbsp;int, required
        <b> Html </b>: &nbsp;&nbsp;string, should contain valid html code
        <b> Created </b>: &nbsp;&nbsp;DatetTime, optional, default is the timestamp of the object creation
        <b> Modified </b>: &nbsp;&nbsp;Datetime, optional, default is null
    }
    </pre>


- DELETE resources:

  - ``DELETE`` **api/banner/{banner_id}** :&nbsp;&nbsp;deletes a specific banner
