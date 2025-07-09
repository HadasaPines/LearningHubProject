import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/loginPage";
import RegisterPage from "./pages/registerPage";
import HomePage from "./pages/homePage";
import RegisterLessonForm from "./pages/student/registerLessonPage"
import ManageAvailability from "./components/admin/manageAvailability";
import ManageStudents from "./components/admin/manageStudents";
import ManageTeachers from "./components/admin/manageTeachers";
import ManageLessons from "./components/admin/manageLessons";
import ManageSubjects from "./components/admin/manageSubjects";
import StudentProfile from "./components/student/studentProfile";
import StudentLessonHistory from "./components/student/lessonsHistory";


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<HomePage />} />
        <Route path="/registerLesson" element={<RegisterLessonForm/>}></Route>
        <Route path="/admin/manage-availability" element={<ManageAvailability />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/admin/manage-students" element={<ManageStudents />} />
       <Route path="/admin/manage-teachers" element={<ManageTeachers />} />
        <Route path="/admin/manage-lessons" element={<ManageLessons />} />
        <Route path="/admin/manage-subjects" element={<ManageSubjects />} />
        <Route path="/student/profile" element={<StudentProfile />} />
        <Route path="/student/lessons-history" element={<StudentLessonHistory />} />
     

      </Routes>
    </Router>
  );
}

export default App;
