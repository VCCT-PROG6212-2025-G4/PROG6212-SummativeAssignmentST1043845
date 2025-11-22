#  CMCS Web Application

##  Overview

The Contract Monthly Claim System (CMCS) is a web-based platform built with ASP.NET Core MVC, Entity Framework Core, and SQL Server to streamline how lecturers submit work-hour claims. The system supports four roles: Lecturer, Coordinator, Manager, and HR, each with unique dashboards and permissions. UI is built using Tailwind CSS, and authentication uses password hashing + session management.
---

##  Key Features

**Lecturers

- Submit claims with hours worked, notes, and optional supporting documents (PDF/DOCX/XLSX).

- Name auto-fills based on login.

- View personal claim history and statuses (Pending/Approved/Rejected).

 **Coordinators

- View all lecturer claims.

- Approve, Reject, or set status to Pending.

- Open lecturer documents and read notes.

 **Managers

- Institution-wide visibility over all claims.

- Oversight and auditing of coordinator actions.

 **HR (Admin Role)

- Full user management (create, edit, delete).

- Adjust lecturer hourly rates using +/− controls.

- Generate Approved Claims Reports with date filtering.

- Generate Invoices (single or batch ZIP).

- Access all approved claim histories.

---

##  Application Structure

###  Controllers

## HomeController
**Purpose- Handles general navigation and landing pages.
**Key Actions
- Index(): Displays the CMCS homepage and navigation options.
- Privacy(): Shows privacy or general information page.

## AuthController
**Purpose Manages system-wide authentication and login sessions.
**Key Actions:
- Login() / Login(vm): Authenticates HR, Lecturers, Coordinators, and Managers.
- Logout(): Clears the session and logs the user out.
- AccessDenied(): Shown when a user attempts to access an unauthorized module.

## LecturerController
**Purpose: Handles all lecturer-specific features.
**Key Actions:
- LecturerDash(): Lecturer dashboard with welcome info.
- SubmitClaim(): Loads the claim submission form pre-filled with lecturer details.
- SubmitClaim(vm): Saves a new claim and uploads supporting documents.
- MyClaims(): Displays all claims submitted by the logged-in lecturer.

## CoordinatorController
**Purpose: Provides claim review and processing tools for coordinators.
Key Actions:
- CoordDash(): Displays all lecturer claims for review.
- Approve(id): Marks a claim as Approved.
- Reject(id): Marks a claim as Rejected.
- SetPending(id):Moves a claim back to Pending for later review.

## ManagerController
**Purpose: Gives managers oversight of all lecturer claims.
Key Actions:
- ManagerDash() – Full list of claims including names, totals, rates, and documents.
- Approve/Reject/SetPending(id) – Secondary verification options if needed.

## HRController
**Purpose: Acts as the administrative core of the CMCS system.
Key Actions:
**User Management:
UserManager():View all users (Lecturer, HR, Manager, Coordinator).
- CreateUser()- CreateUser(vm):Add new users to the system.
- EditUser(id)- EditUser(vm):Modify user details or lecturer rate.
- AdjustRate(lecturerId, value): Increase or decrease lecturer hourly pay.
**Claim Management & Reporting:
- ApprovedClaims(): View all approved lecturer claims.
- ApprovedClaimsReport():Filter approved claims by date and generate summaries.
- GenerateApprovedClaimsReport(): Produce PDF versions of the reports.
- Invoices() – Choose lecturers/range for invoice generation.
- GenerateInvoices() – Create PDFs or ZIP bundles of lecturer invoices.

---

##  Project Structure
CMCS_Web_App/
Controllers/
-Auth, Lecturer, Coordinator, Manager, HR, Home

Models/
 - Claim, Lecturer, User, ViewModels, AppDbContext

Services/
 - ReportService (PDF + HTML reports)

Views/
 - Lecturer, Coordinator, Manager, HR, Shared

wwwroot/
 - uploads/, css/, js/
  
- appsettings.json
- Program.cs

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

##  System Workflow

1.**Lecturer logs in → submits claim.

2.**Claim stored as Pending.

3.**Coordinator reviews → updates status.

4.**Manager oversees all claims.

5.**HR handles reporting, invoices, user admin, and rate management.

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
## AI Declaration 
- Link: https://chatgpt.com/share/69213c23-4e18-8002-8518-81dcea4c17c5 
  Used  chat for layout wiring confirmation
---
## Links
- Youtube Link: https://youtu.be/h61cbrlTth8
- GitHub Link: https://github.com/VCCT-PROG6212-2025-G4/PROG6212-SummativeAssignmentST1043845 
 







