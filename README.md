# BannersRest.API
A minimalistic REST API to manage Html Banners


## Testing the project 

the solution contains two test project:
 - A __UnitTest__ project that tests the behaviour of the REsT ``Controller`` and the `HtmlUtility`
 - An __IntegartionTest__ project which tests the end interaction with the database. 
   * it uses A WebClient that make calls to different REST endpoints and check 
 
 #### Alternatelively 
- You can import the ``Banner.Rest.API.postman_Req_collection.json`` file in [Postman](https://www.getpostman.com/).
It contains a collection of Http requests to interact with the REsT API 


## API Specification:

### Authorization

- Only requests with the headers containing `x-api-key`:`abcde1234` are accepted. 
  * (wanted to keep it simple as we have no concept of users and claims for the moment)

### Endpoints

- GET resources:

- ``GET`` **api/banner**    : get all banners
- ``GET`` **api/banner/{banner_id}** :get specific banner 
- ``GET`` **api/banner/{banner_id}/html** : returns the html content of a specific banner 


- POST resources:

- ``POST`` **api/banner**    : get all banners.

    ** Request Body:
```

  {
    **Id**: int, required
    **Html**: string, should contain valid html code
    **Created**: DatetTime, optional, default is the timestamp of the object creation
    **Modified**: Datetime, optional, default is null
  }

```

- PUT resources:

- ``PUT`` **api/banner/{banner_id}** :Updates a specific banner 


- DELETE resources:

- ``GET`` **api/banner/{banner_id}** :deletes a specific banner
