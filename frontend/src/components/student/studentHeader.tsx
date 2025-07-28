import React, { useState } from "react";
import styles from "./StudentHeader.module.scss";
import { useNavigate } from "react-router-dom";
import {
  FaUserCircle,
  FaUser,
  FaChalkboardTeacher,
  FaWallet,
  FaClipboardList,
  FaSignOutAlt,
} from "react-icons/fa";

const StudentHeader: React.FC = () => {
  const navigate = useNavigate();
  const [showMenu, setShowMenu] = useState(false);

  const handleCloseMenu = () => setShowMenu(false);

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <header className={styles.header}>
      <div className={styles.leftNav}>
        <a onClick={() => navigate("/")}>Home</a>
        <a onClick={() => navigate("/student/Buysubscriptions")}>Buy Subscription</a>
        <a onClick={() => navigate("/registerLesson")}>Register Lesson</a>
        <a onClick={() => navigate("/student/studentHome")}>Dashboard</a>
      </div>

      <div
        className={styles.profileIcon}
        onClick={() => setShowMenu((prev) => !prev)}
      >
        <FaUserCircle />
      </div>

      {showMenu && (
        <div className={styles.profileMenu}>
          <button className={styles.closeButton} onClick={handleCloseMenu}>
            ✕
          </button>

          <div
            className={styles.menuItem}
            onClick={() => {
              navigate("/student/profile");
              handleCloseMenu();
            }}
          >
            <FaUser className={styles.icon} />
            <span>My Profile</span>
          </div>
          <div
            className={styles.menuItem}
            onClick={() => {
              navigate("/student/lessons");
              handleCloseMenu();
            }}
          >
            <FaChalkboardTeacher className={styles.icon} />
            <span>My Lessons</span>
          </div>
          <div
            className={styles.menuItem}
            onClick={() => {
              navigate("/student/payments");
              handleCloseMenu();
            }}
          >
            <FaWallet className={styles.icon} />
            <span>My Payments</span>
          </div>
          <div
            className={styles.menuItem}
            onClick={() => {
              navigate("/student/subscriptions");
              handleCloseMenu();
            }}
          >
            <FaClipboardList className={styles.icon} />
            <span>My Subscriptions</span>
          </div>

          <div className={`${styles.menuItem} ${styles.logout}`} onClick={handleLogout}>
            <FaSignOutAlt className={styles.icon} />
            <span>Logout</span>
          </div>
        </div>
      )}
    </header>
  );
};

export default StudentHeader;
