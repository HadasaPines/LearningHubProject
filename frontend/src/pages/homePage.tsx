import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./HomePage.module.scss";
import TestimonialsSection from "../components/home/testimonialsSection";
import FAQSection from "../components/home/FAQSection";
import OurTeachersSection from "../components/home/ourTeachersSection";

import AboutUs from "../components/home/aboutUs";
import type { User } from "../models/userModel";
import OurSubjectsSection from "../components/home/ourSubjectsSection";

const HomePage: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const links = document.querySelectorAll('a[href^="#"]');
    const handleClick = (e: Event) => {
      e.preventDefault();
      const href = (e.currentTarget as HTMLAnchorElement).getAttribute("href");
      const target = href && document.querySelector(href);
      if (target) {
        target.scrollIntoView({ behavior: "smooth" });
      }
    };

    links.forEach((link) => link.addEventListener("click", handleClick));
    return () => {
      links.forEach((link) => link.removeEventListener("click", handleClick));
    };
  }, []);

const handleAccountClick = () => {
  const user = localStorage.getItem("user");

  if (!user) {
    navigate("/auth");
    return;
  }

  const userData: User = JSON.parse(user);
  if (userData.role === "Student") {
    navigate("/student/studentHome");
  }

  if (userData.role === "Admin") {
    navigate("/Admin/adminHome");
  }
};


  return (
    <div className={styles.homePage}>
      <header className={styles.header}>
        <nav className={styles.navbar}>
          <div className={styles.leftMenu}>
            <a href="#hero">Home</a>
            <a href="#testimonials">Testimonials</a>
            <a href="#faq">FAQ</a>
            <a href="#our-teachers" >Our Teachers</a>
            <a href="#our-subjects">Our Subjects</a>
            <a href="#about-us">About Us</a>
            
          </div>
          <div className={styles.rightMenu}>
            <button className={styles.accountButton} onClick={handleAccountClick}>
              My Account
            </button>
          </div>
        </nav>
      </header>

      <main className={styles.mainContent}>
       
       

        <div className={styles.overlay}>
           <section id="hero" className={styles.heroImage}></section>
          <section id="testimonials">
            <TestimonialsSection />
          </section>

          <section id="faq">
            <FAQSection />
          </section>

          <section id="our-teachers">
            <OurTeachersSection />
          </section>

          <section id="our-subjects">
            <OurSubjectsSection />
          </section>

          <section id="about-us">
            <AboutUs />
          </section>

         
        </div>
      </main>
    </div>
  );
};

export default HomePage;
