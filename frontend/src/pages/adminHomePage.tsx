import React, { useState } from "react";
import styles from "./AdminHomePage.module.scss";
import { useNavigate } from "react-router-dom";
import {
  FaUserCircle,
  FaUsers,
  FaChalkboardTeacher,
  FaCalendarAlt,
  FaClipboardList,
  FaClock,
  FaSignOutAlt,
} from "react-icons/fa";

import ManageAvailability from "../components/admin/manageAvailability";
import ManageStudents from "../components/admin/manageStudents";
import ManageTeachers from "../components/admin/manageTeachers";
import ManageLessons from "../components/admin/manageLessons";
import ManageSubscriptions from "../components/admin/manageSubscriptions";

const AdminHomePage: React.FC = () => {
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
      case "students":
        return <ManageStudents />;
      case "teachers":
        return <ManageTeachers />;
      case "lessons":
        return <ManageLessons />;
      case "availability":
        return <ManageAvailability />;
      case "subscriptions":
        return <ManageSubscriptions />;
      case "dashboard":
      default:
        return (
          <div className={styles.dashboard}>
            <h2>Welcome, Admin</h2>

            <div className={styles.adminButtons}>
              {/* שורה עליונה – 3 כפתורים */}
              <div className={styles.row}>
                <button onClick={() => handleMenuClick("students")}>
                  <FaUsers className={styles.icon} />
                  Manage Students
                </button>
                <button onClick={() => handleMenuClick("teachers")}>
                  <FaChalkboardTeacher className={styles.icon} />
                  Manage Teachers
                </button>
                <button onClick={() => handleMenuClick("lessons")}>
                  <FaCalendarAlt className={styles.icon} />
                  Manage Lessons
                </button>
              </div>

              {/* שורה תחתונה – 2 כפתורים ממורכזים מתחת לשלושת העליונים */}
              <div className={`${styles.row} ${styles.centered}`}>
                <button onClick={() => handleMenuClick("availability")}>
                  <FaClock className={styles.icon} />
                  Manage Availability
                </button>
                <button onClick={() => handleMenuClick("subscriptions")}>
                  <FaClipboardList className={styles.icon} />
                  Manage Subscriptions
                </button>
              </div>
            </div>
          </div>
        );
    }
  };

  return (
    <div className={styles.homePage}>
      <header className={styles.header}>
        <div className={styles.leftNav}>
          <a onClick={() => navigate("/")}>Home</a>
          <a onClick={() => handleMenuClick("dashboard")}>Dashboard</a>
        </div>

        <div
          className={styles.profileIcon}
          onClick={() => setShowMenu(!showMenu)}
        >
          <FaUserCircle />
        </div>

        {showMenu && (
          <div className={styles.profileMenu}>
            <button className={styles.closeButton} onClick={handleCloseMenu}>
              ✕
            </button>

            <div
              className={`${styles.menuItem} ${styles.logout}`}
              onClick={handleLogout}
            >
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

export default AdminHomePage;
