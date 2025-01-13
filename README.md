# **Technical Design Document**

## **1. Background**
The goal of this project is to create a backend tool that syncs contacts from MockAPI to Mailchimp.

### **1.1 Requirements**
- Retrieve contact data (first name, last name, and email) from MockAPI.
- Add these contacts as members of a new list in Mailchimp.
- Name the Mailchimp list with the developer’s personal name.
- Provide a RESTful endpoint `/contacts/sync` to sync and return the total number of synced contacts and their details

### **1.2 Clean Architecture**
- The project is built following **Clean Architecture** principles:
  - **Separation of Concerns:** Each layer (Presentation, Application, Domain, Infrastructure) has a clear responsibility.
  - **SOLID Principles:** Ensures flexibility, maintainability, and testability.
  - **Dependency Inversion:** Interfaces abstract dependencies, decoupling implementations from higher-level business logic.


## **2. Technical Approaches**

### **2.1 Layers and Project Structure**
#### **2.1.1 Presentation Layer (API)**
- Manages RESTful endpoints.
- **Main Endpoint:** `GET /contacts/sync` to initiate the sync process.
- **Key Files:** `ContactsController.cs`.

#### **2.1.2 Application Layer**
- Orquestrade the domain business logic for synchronization.
- **Service:** `ContactSyncService`
  - Fetches data from Domain.
  - Process use cases and validates the data.
  - Return information to presentation.
- **Key Files:** `ContactSyncService.cs`.

#### **2.1.3 Domain Layer**
- Defines core entities such as `Contact`, `SyncResult`.
- Own infrastrucuture interfaces like `IContactService`, `IMailService`.
- Invokes the infrastructure layer to get/send data to external services.
- **Key Files:** `IContactService.cs`, `IMailService.cs`, `Contact.cs`

#### **2.1.4 Infrastructure Layer**
- Implements communication with external APIs (MockAPI and Mailchimp).
- Uses `HttpClient` to handle requests.
- Specific clients: `MailchimpClient`, `MockApiClient`.
- **Key Files:** `MailchimpClient.cs`, and `MockApiClient.cs`

### **2.2 Technical Decisions**
1. **Use of DTOs (Data Transfer Objects):**
   - DTOs such as `ContactDTO` and `SyncResponseDTO` are used to isolate the data transferred between layers and the API response.

2. **Dependency Injection (DI):**
   - Configured in the `DependencyInjection.cs` file to manage dependencies like `IMailchimpClient` and `IContactSyncService`.

3. **Modularity and Flexibility:**
   - The infrastructure layer is designed to handle external dependencies (e.g., Mailchimp or databases). Interfaces ensure that changing or extending these dependencies (e.g., swapping Mailchimp for another service) requires minimal code changes.

4. **Testability:**
   - This architecture allows each layer to be tested independently, ensuring that functionalities are validated in isolation without dependencies on other layers.
   - Mocks (`MockApiClient`) and fakers (`ContactDtoFaker`) simulate real scenarios in unit tests, enabling isolated and effective testing of each layer.

6. **Error Handling:**
   - Encapsulated in the `BaseHttpClient` to standardize responses and handle failures when communicating with external APIs.


## **3. Workflow**

### **3.1 Sync Process**
1. **Request to the Endpoint:**
   - A client sends a `POST` request to the `/contacts/sync` endpoint.

2. **Fetching Data from MockAPI:**
   - The application calls `MockApiClient` to fetch contact data.
   - The response is mapped into `ContactDTO`.

3. **Creating Members in Mailchimp:**
   - For each contact retrieved, `MailchimpClient` sends a request to add the contact to a specific list.

4. **Building the Response:**
   - The `ContactSyncService` returns the total number of synced contacts and their details in JSON format.

### **3.2 Example Response**
```json
{
  "syncedContacts": 3,
  "contacts": [
    {
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com"
    },
    {
      "firstName": "Jane",
      "lastName": "Smith",
      "email": "jane.smith@example.com"
    }
  ]
}
```

## **4. Files and Classes**

### **4.1 Key Files Modified**
1. **Presentation Layer:**
   - `ContactsController.cs`: Defines the `/contacts/sync` endpoint. Responsible for handling requests from clients and initiating the synchronization process through the application layer.

2. **Application Layer:**
   - `ContactSyncService.cs`: Contains the main business logic for synchronizing contacts between MockAPI and Mailchimp. Handles data validation and coordinates operations with the domain and infrastructure layers.

3. **Infrastructure Layer:**
   - `MailchimpClient.cs`: Implements the communication with the Mailchimp API to create and manage mailing lists and contacts.
   - `MockApiClient.cs`: Fetches contacts from the MockAPI, translating their structure into usable DTOs for the application layer.

### **4.2 Data Structure**
#### **4.2.1 Entity `Contact` (Domain Layer):**
- Represents a contact in the domain model.
- **Fields:**
  - `FirstName` (string): The first name of the contact.
  - `LastName` (string): The last name of the contact.
  - `Email` (string): The email address of the contact.

#### **4.2.2 DTO `ContactDTO`:**
- Used to transfer contact data between layers without exposing the domain entity directly.
- **Fields:**
  - `FirstName` (string)
  - `LastName` (string)
  - `Email` (string)

#### **4.2.3 DTO `SyncResponseDTO`:**
- Represents the response structure for the `/contacts/sync` endpoint.
- **Fields:**
  - `SyncedContacts` (int): The total number of contacts successfully synchronized.
  - `Contacts` (list of `ContactDTO`): The details of all synchronized contacts.


## **5. Hosting**
- The project DEMO is hosted on a free cloud platform for accessibility.
   - http://mailsyncer.runasp.net/


## **6. Api Documentation**
- The API is documented using `scalar.com`, atool with a UI similar to Postman or Swagger, allowing for easy endpoint testing and interaction.
### **6.1 Endpoints**
- **Synchronize Contacts**
  - **URL:** `/api/contacts/sync`
  - **Method:** `POST`
  - **Description:** Synchronizes contacts from MockAPI to Mailchimp and returns the total number of synchronized contacts along with their details.
  - **Response Codes:**
    - `200 OK`: Operation succeeded, and data was synchronized.
    - `204 No Content`: No contacts were found to synchronize.

- **Get Contacts**
  - **URL:** `/api/contacts/get`
  - **Method:** `GET`
  - **Description:** Retrieves all contacts currently synchronized.
  - **Response Codes:**
    - `200 OK`: Operation succeeded, and data was retrieved.
    - `204 No Content`: No contacts were found.

- **Clean Contacts**
  - **URL:** `/api/contacts/clean`
  - **Method:** `DELETE`
  - **Description:** Deletes all synchronized contacts and returns the operation result.
  - **Response Codes:**
    - `200 OK`: Operation succeeded, and contacts were deleted.
    - `204 No Content`: No contacts were found to delete.


## **7. Apresentation**
- A recorded video that provides an overview of how the software works and explains key aspects of the code, including its architecture, functionality, and design decisions:
https://www.loom.com/share/0e5c91e7f1df452dab100b6125cb7b47


## **8. Next Steps**
1. Implement authentication and authorization to ensure secure and authorized access to the system.
2. Consider adding centralized logging to monitor failures in external integrations.
3. Explore additional test coverage to handle edge cases or integration-level testing.
