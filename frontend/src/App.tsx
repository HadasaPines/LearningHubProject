import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/loginPage";
import RegisterPage from "./pages/registerPage";
import HomePage from "./pages/homePage";
import RegisterLessonForm from "./pages/student/registerLessonPage"

import ManageStudents from "./pages/admin/manageStudents";
import ManageLessons from "./pages/admin/manageLessons";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<HomePage />} />
        <Route path="/registerLesson" element={<RegisterLessonForm/>}></Route>

        <Route path="/home" element={<HomePage />} />
        <Route path="/admin/manage-students" element={<ManageStudents />} />
        <Route path="/admin/manage-lessons" element={<ManageLessons />} />
        {/* Add more routes as needed */}
      </Routes>
    </Router>
  );
}

export default App;
