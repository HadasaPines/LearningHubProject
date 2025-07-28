import React, { useState } from 'react';
import styles from './FAQSetion.module.scss';

interface Question {
  q: string;
  a: string;
}

const questions: Question[] = [
  { q: "How do I register?", a: "You can register through the signup or login page." },
  { q: "Can I cancel a lesson?", a: "Yes, you can cancel a lesson by coordinating with your teacher." },
  { q: "Is there a mobile app?", a: "Yes, our mobile app is available for iOS and Android." },
  { q: "How do I contact support?", a: "You can contact support via email at support@yoani.com." },
  { q: "Are the lessons recorded?", a: "Yes, most lessons are recorded and available in your dashboard." },
  { q: "What languages are supported?", a: "Currently, we support Hebrew, English, and Arabic." },
  { q: "Can I change my teacher?", a: "Yes, you can request a teacher change in your settings." },
  { q: "How do I reset my password?", a: "Click 'Forgot Password' on the login page and follow the instructions." },
  { q: "Is there a free trial?", a: "Yes, we offer a 7-day free trial for new users." },
  { q: "Can I access lessons offline?", a: "Offline access is available on our mobile app with downloads." },
  { q: "How do I change my subscription?", a: "Go to your account settings and select 'Subscription'." },
  { q: "What payment methods do you accept?", a: "We accept major credit cards and PayPal." },
];

const FAQSection: React.FC = () => {
  const [openIndex, setOpenIndex] = useState<number | null>(null);

  const toggle = (index: number) => {
    setOpenIndex(openIndex === index ? null : index);
  };

  const midIndex = Math.ceil(questions.length / 2);
  const leftColumn = questions.slice(0, midIndex);
  const rightColumn = questions.slice(midIndex);

  return (
    <section className={styles.faqSection}>
      <h2 className={styles.title}>Frequently Asked Questions</h2>
      <div className={styles.faqGrid}>
        <ul className={styles.faqList}>
          {leftColumn.map((qa, i) => (
            <li key={i} className={styles.faqItem}>
              <button className={styles.question} onClick={() => toggle(i)}>
                {qa.q}
                <span
                  className={`${styles.arrow} ${openIndex === i ? styles.arrowOpen : ''}`}
                >
                  ▶
                </span>
              </button>
              <div className={`${styles.answer} ${openIndex === i ? styles.answerOpen : ''}`}>
                {qa.a}
              </div>
            </li>
          ))}
        </ul>

        <ul className={styles.faqList}>
          {rightColumn.map((qa, i) => (
            <li key={i + midIndex} className={styles.faqItem}>
              <button className={styles.question} onClick={() => toggle(i + midIndex)}>
                {qa.q}
                <span
                  className={`${styles.arrow} ${openIndex === i + midIndex ? styles.arrowOpen : ''}`}
                >
                  ▶
                </span>
              </button>
              <div className={`${styles.answer} ${openIndex === i + midIndex ? styles.answerOpen : ''}`}>
                {qa.a}
              </div>
            </li>
          ))}
        </ul>
      </div>
    </section>
  );
};

export default FAQSection;
