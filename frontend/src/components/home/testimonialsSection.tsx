
import React, { useState } from "react";
import styles from "./testimonialsSection.module.scss";

type Testimonial = {
  name: string;
  text: string;
  rating: number;
};

const initialTestimonials: Testimonial[] = [
  { name: "Dana", text: "My son hasn't missed a class since we joined!", rating: 5 },
  { name: "Moshe", text: "Friendly service and excellent teachers!", rating: 4 },
];

const TestimonialsSection = () => {
  const [testimonials, setTestimonials] = useState<Testimonial[]>(initialTestimonials);
  const [name, setName] = useState("");
  const [text, setText] = useState("");
  const [rating, setRating] = useState(0);
  const [hoveredRating, setHoveredRating] = useState<number | null>(null);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!name || !text || rating === 0) return;

    const newTestimonial: Testimonial = { name, text, rating };
    setTestimonials([...testimonials, newTestimonial]);

    setName("");
    setText("");
    setRating(0);
    setHoveredRating(null);
  };

  const renderStars = (
    value: number,
    clickable = false,
    onClick?: (i: number) => void,
    onHover?: (i: number | null) => void
  ) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      const filled = i <= value;
      stars.push(
    <span
  key={i}
  onClick={clickable ? () => onClick?.(i) : undefined}
  onMouseEnter={clickable ? () => onHover?.(i) : undefined}
  onMouseLeave={clickable ? () => onHover?.(null) : undefined}
  className={`${styles.star} ${filled ? styles.filled : ""}`}
>
  ★
</span>

      );
    }
    return stars;
  };

  return (
    <section id="testimonials" className={styles.testimonialsSection}>
      <h2 className={styles.title}>What Our Users Say</h2>
      <div className={styles.testimonialsLayout}>
        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.formGroup}>
            <label className={styles.label}>Your Name:</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className={styles.input}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label className={styles.label}>Your Testimonial:</label>
            <textarea
              value={text}
              onChange={(e) => setText(e.target.value)}
              className={styles.textarea}
              placeholder="Share your experience..."
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label className={styles.label}>Your Rating:</label>
            <div className={styles.stars}>
              {renderStars(hoveredRating ?? rating, true, setRating, setHoveredRating)}
            </div>
          </div>

          <button type="submit" className={styles.submitButton}>
            Submit Testimonial
          </button>
        </form>

        <div className={styles.testimonialsList}>
          {testimonials.map((t, i) => (
            <blockquote key={i} className={styles.testimonial}>
              <p className={styles.text}>"{t.text}"</p>
              <footer className={styles.footer}>
                <span>{t.name}</span>
                <span className={styles.stars}>{renderStars(t.rating)}</span>
              </footer>
            </blockquote>
          ))}
        </div>
      </div>
    </section>
  );
};

export default TestimonialsSection;