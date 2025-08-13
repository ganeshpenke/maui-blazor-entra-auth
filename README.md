# maui-blazor-entra-auth
A **.NET MAUI Blazor Hybrid** application secured with **Azure Entra ID** authentication using the **Microsoft Identity Platform** for sign-in and protected API access.

This project demonstrates how to:
- Integrate **Azure Entra ID** authentication in a .NET MAUI Blazor Hybrid app.
- Implement login/logout flow with account selection.

---

## 📸 Screenshots

### 1. Unauthorized Access Prompt
When accessing a protected page without signing in:
<img width="1919" height="1028" alt="image" src="https://github.com/user-attachments/assets/15e0c87d-e42f-4661-99d2-cb31be610ce7" />

---

### 2. Account Selection (Azure Entra ID Login)
User chooses an account to sign in:
<img width="1899" height="944" alt="image" src="https://github.com/user-attachments/assets/3d2c54c2-54a1-46b9-95fb-2e003ae369a6" />

---

### 3. Authenticated Home Page
After a successful login:
<img width="1919" height="1027" alt="image" src="https://github.com/user-attachments/assets/9b4461fa-952b-45db-8433-41cb2b2b2b78" />

---

## 🚀 Features
- **Azure Entra ID Authentication** via Microsoft Identity Platform.
- **Protected Routes** – Only accessible when logged in.
- **Account Picker UI** for seamless switching.
- Works across **Windows, Mac, Android, iOS**.

---

## 🛠 Setup

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 (with MAUI & Blazor workload)
- Azure Entra ID App Registration

---

### 2. Azure Entra ID Configuration

1. Go to [portal.azure.com](https://portal.azure.com) and search for **"App Registrations"**.
2. Click **"New Registration"**.
3. Enter a name for your app registration.
4. Select the platform **"Public client/native (mobile & desktop)"** and register your application.  
   ✅ Once created, your app registration will appear in the list.

## Configure Authentication Settings

1. Click on the **"Authentication"** tab in your app registration.
2. Under **Redirect URIs**, make sure the following are added:  
   - `https://login.microsoftonline.com/common/oauth2/nativeclient` *(for native client apps)*  
   - `https://localhost:5002` *(or another available port on your system)*  

   <img width="1010" height="365" alt="image" src="https://github.com/user-attachments/assets/cb908e3c-5442-4778-9d6c-8940526007fe" />

3. Save the configuration.

---

### 3. Run the App

```bash
dotnet build
dotnet run


