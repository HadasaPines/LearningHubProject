import React, { useState } from "react";
import styles from "./StudentHomePage.module.scss";
import { useNavigate } from "react-router-dom";
import {FaUserCircle, FaUser, FaChalkboardTeacher, FaWallet, FaClipboardList, FaSignOutAlt } from "react-icons/fa";


import StudentProfile from "../components/student/studentProfile";
import StudentLessonHistory from "../components/student/lessonsHistory";
import StudentSubscriptions from "../components/student/studentSubscriptions";
import StudentPayments from "../components/student/paymentsHistory";
import DashboardComponent from "../components/student/studentDashboard";

const StudentHomePage: React.FC = () => {
  const navigate = useNavigate();
  const [selectedComponent, setSelectedComponent] = useState<string>("dashboard");
  const [showMenu, setShowMenu] = useState(false);

  const handleCloseMenu = () => setShowMenu(false);

  const handleMenuClick = (component: string) => {
    setSelectedComponent(component);
    setShowMenu(false);
  };

  const handleLogout = () => {
    localStorage.clear(); 
    navigate("/"); 
  };

  const renderComponent = () => {
    switch (selectedComponent) {
      case "dashboard":
        return <DashboardComponent />;
      case "profile":
        return <StudentProfile />;
      case "lessons":
        return <StudentLessonHistory />;
      case "subscriptions":
        return <StudentSubscriptions />;
      case "payments":
        return <StudentPayments />;
      default:
        return null;
    }
  };

  return (
    <div className={styles.homePage}>
      <header className={styles.header}>

        <div className={styles.leftNav}>
          <a onClick={() => navigate("/")}>Home</a>
          <a onClick={() => navigate("/student/Buysubscriptions")}>Buy Subscription</a>
          <a onClick={() => navigate("/registerLesson")}>Register Lesson</a>
          <a onClick={() => handleMenuClick("dashboard")}>Dashboard</a>
        </div>

        <div className={styles.profileIcon} onClick={() => setShowMenu(!showMenu)}>
          <FaUserCircle />
        </div>

       {showMenu && (
  <div className={styles.profileMenu}>
    <button className={styles.closeButton} onClick={handleCloseMenu}>✕</button>

    <div className={styles.menuItem} onClick={() => handleMenuClick("profile")}>
      <FaUser className={styles.icon} />
      <span>My Profile</span>
    </div>
    <div className={styles.menuItem} onClick={() => handleMenuClick("lessons")}>
      <FaChalkboardTeacher className={styles.icon} />
      <span>My Lessons</span>
    </div>
    <div className={styles.menuItem} onClick={() => handleMenuClick("payments")}>
      <FaWallet className={styles.icon} />
      <span>My Payments</span>
    </div>
    <div className={styles.menuItem} onClick={() => handleMenuClick("subscriptions")}>
      <FaClipboardList className={styles.icon} />
      <span>My Subscriptions</span>
    </div>

    <div className={styles.menuItem + " " + styles.logout} onClick={handleLogout}>
      <FaSignOutAlt className={styles.icon} />
      <span>Logout</span>
    </div>
  </div>
)}

      </header>

      <main className={styles.mainContent}>
        {renderComponent()}
      </main>
    </div>
  );
};

export default StudentHomePage;
