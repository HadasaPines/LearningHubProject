
import { FaLightbulb, FaCompass, FaHandshake } from 'react-icons/fa'; 
import styles from './AboutUs.module.scss';

const AboutUs = () => {
  return (
    <div className={styles.aboutUs}>
      <header className={styles.aboutUsHeader}>
        <h1>About Us</h1>
        <p>We offer personalized tutoring services to help you succeed. Whether you're preparing for exams or learning new skills, we are here to guide you every step of the way.</p>
      </header>

      <section className={styles.aboutUsSection}>
        <div className={styles.card}>
          <div className={styles.icon}>
            <FaLightbulb />
          </div>
          <h2>Our Vision</h2>
          <p>Our vision is to make high-quality education accessible to everyone. We provide tailored lessons to match your learning pace, helping you achieve your academic and personal goals.</p>
        </div>
        <div className={styles.card}>
          <div className={styles.icon}>
            <FaCompass />
          </div>
          <h2>What We Believe</h2>
          <p>We believe that every student has unique needs. Our approach focuses on understanding your strengths and areas for improvement, while building confidence and motivation throughout your learning journey.</p>
        </div>
        <div className={styles.card}>
          <div className={styles.icon}>
            <FaHandshake />
          </div>
          <h2>Join Us</h2>
          <p>Become part of our supportive learning community. Sign up for personalized lessons, track your progress, and grow with us as we work together to achieve your goals.</p>
        </div>
      </section>
    </div>
  );
};

export default AboutUs;
