#  CMCS Web Application

##  Overview

The **Claim Management and Coordination System (CMCS)** is a web-based platform built with **ASP.NET Core MVC** and **Entity Framework Core** to streamline and manage lecturer claim submissions at an academic institution.  
The system enables **Lecturers** to submit and track claims, while **Coordinators** and **Managers** can review, approve, reject, or hold claims for further evaluation.

CMCS provides a clear workflow for claim management, status tracking, and reporting through a modern and responsive web interface styled with **Tailwind CSS**.

---

##  Key Features

-  **Lecturer Dashboard**
  - Submit new claims with details such as hours worked, hourly rate, and notes.
  - Upload supporting documents for verification.
  - View all personal claims with timestamps and their current statuses (Pending, Approved, or Rejected).

-  **Coordinator Dashboard**
  - Login required to access.
  - Review all lecturer claims in the system.
  - Approve, reject, or mark claims as pending (for later review).

-  **Manager Dashboard**
  - Login required to access.
  - View all claims and their statuses for oversight.
  - Option to finalize or audit coordinator actions.

-  **Reports Page (Home Dashboard)**
  - Displays total claims count and status breakdown (Total, Approved, Pending, Rejected).
  - Links directly to a reporting view showing claim summaries.

---

##  Application Structure

###  Controllers

| Controller | Purpose | Key Actions |

| **HomeController** | Handles the main landing page and general site navigation. | `Index()` – Displays home page with CMCS overview and navigation buttons.<br>`Privacy()` – Displays privacy policy. |
| **LecturerController** | Manages lecturer operations: claim submission and viewing. | `Index()` – Displays all lecturers.<br>`SubmitClaim()` – Allows a lecturer to create and upload a claim.<br>`MyClaims()` – Shows all claims submitted by the logged-in lecturer.<br>`Edit/Delete/Details()` – Standard CRUD for lecturer data. |
| **CoordinatorController** | Provides tools for coordinators to manage submitted claims. | `Login()` – Coordinator authentication.<br>`CoordDash()` (or `Index()`) – Dashboard listing all claims.<br>`ApproveClaim()`, `RejectClaim()`, `SetPending()` – Update claim statuses. |
| **ManagerController** | Allows managers to review all claims for audit and final approval. | `Login()` – Manager authentication.<br>`ManagerDash()` (or `Index()`) – Displays all claims and their current statuses. |
| **ReportsController** | Generates statistical summaries of all claims for quick insight. | `Index()` – Shows total, approved, pending, and rejected claim counts.<br>Optional filters for claim status and export options. |

---

##  Project Structure
CMCS_Web_App/
├── Controllers/
│ ├── HomeController.cs
│ ├── LecturerController.cs
| ├── HRController.cs
│ ├── CoordinatorController.cs
│ ├── ManagerController.cs
│ └── ReportsController.cs
│
├── Models/
│ ├── Claim.cs
│ ├── Lecturer.cs
│ ├── ClaimStatus.cs
│ ├── Reports.cs
│ └── AppDbContext.cs
│  
├── Views/
│ ├── Home/
│ ├── Lecturer/
│ ├── Coordinator/
│ ├── Manager/
│ └── Reports/
│
├──Services/
| ReportService.cs
├── wwwroot/
│ ├── css/
│ ├── js/
│ └── uploads/
│
├── appsettings.json
├── Program.cs


---

##  Technologies Used

### **Backend**

* **ASP.NET Core MVC** – Main web framework used to build the application.
* **Entity Framework Core** – ORM for database read/write operations.
* **SQLite / SQL Server** – Database engine (depending on your configuration).
* **Dependency Injection** – Used for services such as PDF conversion and database context.

### **Frontend**

* **Razor Views (.cshtml)** – Dynamic HTML pages with C#.
* **TailwindCSS or Bootstrap** – For responsive UI and layout.

### **PDF Generation**

* **DinkToPdf** – Generates PDF versions of claim reports.
 ---

##  Claim Workflow

1. **Lecturer** logs in and submits a claim.  
2. **Claim** is stored in the database with status = `Pending`.  
3. **Coordinator** reviews and changes status to `Approved`, `Rejected`, or keeps it `Pending`.  
4. **Manager** has oversight and can review all processed claims.  
5. **Reports page** displays claim statistics for transparency.

---

## Login details 
- UserId = 1,
   Email = "Lofentse13@CMCSLEC.com",
   Password-0628
- UserId = 2,                   
   Email = "Karabo28@CMCSLEC.com",                
     Password-1113                
-  UserId = 3,
   Email = "Claudia06@CMCSLEC.com",
   Password-0731
-  UserId = 4,
    Email = "Co-ordinator@CoordCMCS.com",              
    Password-4639                    
 -   UserId = 5,
    Email = "Manager@ManCMCS.com",        
    Password-1243                
-  UserId = 6,
   Email = "HR@ResourcesCMCS.com",       
   Password-0432                                    
---
# Lecturer Feedback

- Lecturers do not define their hourly rate. Please ensure that HR sets up hourly rates for lecturers in Part 3.
 -  I did rectify this by adding HR function to my web app and enabled HR to be the only user allowed to edit hourly rate.
-  Action buttons on the Coordinator and Manager dashboards are not well aligned. 
> Coordinators and Managers must have the option to fully view all the details of a claim. How does one view a supporting doc?
> Managers should only be able to action claims that were previously approved/verified by Coordinators. Nothing else.
  - I aligned Co-ordinator and manager accordingly.
  - There is a clickable view button on the claims table that allows you to view the supporting doc.
- No file type and size validation. Files are saved publicly in wwwroot/uploads without encryption or authorisation, with minimal error handling (no IO try/catch, request-size limits,      etc.)
    - Added file validation.
  
                   
---

## Links
- Youtube Link: https://youtu.be/h61cbrlTth8
- GitHub Link: https://github.com/VCCT-PROG6212-2025-G4/PROG6212-SummativeAssignmentST1043845 
 







