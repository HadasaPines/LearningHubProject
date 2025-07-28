import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import HomePage from "./pages/homePage";


import AuthPage from "./pages/authPage";


import StudentHomePage from "./pages/studentHomePage";
import RegisterLessonForm from "./pages/student/registerLessonPage";
import BuyStudentSubscriptions from "./components/student/buyStudentSubscriptions";
import AdminHomePage from "./pages/adminHomePage";




function App() {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<HomePage />} />
      <Route path="/auth" element={<AuthPage />} />
      <Route path="student/studentHome" element={<StudentHomePage/>}/>
      <Route path="registerLesson" element={<RegisterLessonForm/>}/>
      <Route path="student/Buysubscriptions" element={<BuyStudentSubscriptions/>}/>
      
      <Route path="admin/adminHome" element={<AdminHomePage/>}/>

        


  

        <Route path="/home" element={<HomePage />} />



      </Routes>
    </Router>
  );
}

export default App;