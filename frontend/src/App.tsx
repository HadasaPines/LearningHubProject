import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import HomePage from "./pages/homePage";

import ManageAvailability from "./components/admin/manageAvailability";
import ManageSubjects from "./components/admin/manageSubjects";
import ManageStudents from "./components/admin/manageStudents";
import ManageTeachers from "./components/admin/manageTeachers";
import ManageLessons from "./components/admin/manageLessons";
import ManageSubscriptions from "./components/admin/manageSubscriptions";
import AuthPage from "./pages/authPage";


import StudentHomePage from "./pages/studentHomePage";
import RegisterLessonForm from "./pages/student/registerLessonPage";
import BuyStudentSubscriptions from "./components/student/buyStudentSubscriptions";



function App() {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<HomePage />} />
      <Route path="/auth" element={<AuthPage />} />
      <Route path="student/studentHome" element={<StudentHomePage/>}/>
      <Route path="registerLesson" element={<RegisterLessonForm/>}/>
      <Route path="student/Buysubscriptions" element={<BuyStudentSubscriptions/>}/>
      

        <Route path="/admin/manage-availability" element={<ManageAvailability />} />
        <Route path="/admin/manage-students" element={<ManageStudents />} />
        <Route path="/admin/manage-teachers" element={<ManageTeachers />} />
        <Route path="/admin/manage-lessons" element={<ManageLessons />} />
        <Route path="/admin/manage-subjects" element={<ManageSubjects />} />
        <Route path="/admin/manage-subscription" element={<ManageSubscriptions />} />



  

        <Route path="/home" element={<HomePage />} />



      </Routes>
    </Router>
  );
}

export default App;