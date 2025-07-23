import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import HomePage from "./pages/homePage";
import RegisterLessonForm from "./pages/student/registerLessonPage"
import StudentSubscriptions from "./components/student/studentSubscriptions";
import ManageAvailability from "./components/admin/manageAvailability";
import ManageSubjects from "./components/admin/manageSubjects";
import ManageStudents from "./components/admin/manageStudents";
import ManageTeachers from "./components/admin/manageTeachers";
import ManageLessons from "./components/admin/manageLessons";
import StudentProfile from "./components/student/studentProfile";
import StudentLessonHistory from "./components/student/lessonsHistory";
import TestimonialsSection from "./components/home/testimonialsSection";
import CallToActionSection from "./components/home/callToActionSection";
import FAQSection from "./components/home/FAQSection";
import OurTeachersSection from "./components/home/ourTeachersSection";
import OurLessonsSection from "./components/home/ourLessonsSection";
import ManageSubscriptions from "./components/admin/manageSubscriptions";
import StudentPayments from "./components/student/paymentsHistory";
import BuyStudentSubscriptions from "./components/student/buyStudentSubscriptions";
import AuthPage from "./pages/authPage";


function App() {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<HomePage />} />
      <Route path="/auth" element={<AuthPage />} />

        <Route path="/admin/manage-availability" element={<ManageAvailability />} />
        <Route path="/admin/manage-students" element={<ManageStudents />} />
        <Route path="/admin/manage-teachers" element={<ManageTeachers />} />
        <Route path="/admin/manage-lessons" element={<ManageLessons />} />
        <Route path="/admin/manage-subjects" element={<ManageSubjects />} />
        <Route path="/admin/manage-subscription" element={<ManageSubscriptions />} />

        <Route path="/registerLesson" element={<RegisterLessonForm />}></Route>
        <Route path="/student/profile" element={<StudentProfile />} />
        <Route path="/student/lessons-history" element={<StudentLessonHistory />} />
        <Route path="/student/buySubscriptions" element={<BuyStudentSubscriptions />} />
        <Route path="/student/mySubscriptions" element={<StudentSubscriptions />} />
        <Route path="/student/payments" element={<StudentPayments />} />

        <Route path="/home" element={<HomePage />} />
        <Route path="/home/testimonials" element={<TestimonialsSection />} />
        <Route path="/home/call-to-action" element={<CallToActionSection />} />
        <Route path="/home/FAQ" element={<FAQSection />} />
        <Route path="/home/our-teachers" element={<OurTeachersSection />} />
        <Route path="/home/our-lessons" element={<OurLessonsSection />} />


      </Routes>
    </Router>
  );
}

export default App;