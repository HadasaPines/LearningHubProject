import React, { useState } from "react";

type Testimonial = {
  name: string;
  text: string;
  rating: number;
};

const initialTestimonials: Testimonial[] = [
  { name: "דנה", text: "הבן שלי לא מפספס שיעור מאז שנרשמנו!", rating: 5 },
  { name: "משה", text: "שירות נעים ומורים מעולים!", rating: 4 },
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

  const renderStars = (value: number, clickable = false, onClick?: (i: number) => void, onHover?: (i: number | null) => void) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      const filled = i <= value;
      stars.push(
        <span
          key={i}
          onClick={clickable ? () => onClick?.(i) : undefined}
          onMouseEnter={clickable ? () => onHover?.(i) : undefined}
          onMouseLeave={clickable ? () => onHover?.(null) : undefined}
          style={{
            cursor: clickable ? "pointer" : "default",
            color: filled ? "#FFD700" : "#ccc",
            fontSize: "1.5rem",
          }}
        >
          ★
        </span>
      );
    }
    return stars;
  };

  return (
    <section className="p-10 bg-white" dir="rtl">
      <h2 className="text-2xl font-bold mb-4">תגובות:</h2>


      <form onSubmit={handleSubmit} className="mb-6 bg-gray-100 p-4 rounded shadow">
        <div className="mb-2">
          <label className="block font-bold">שם:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded"
            required
          />
        </div>

        <div className="mb-2">
          <label className="block font-bold">תגובה:</label>
          <textarea
            value={text}
            onChange={(e) => setText(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded"
            required
          />
        </div>

        <div className="mb-2">
          <label className="block font-bold">דירוג:</label>
          <div>
            {renderStars(hoveredRating ?? rating, true, setRating, setHoveredRating)}
          </div>
        </div>

        <button
          type="submit"
          className="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          שלח המלצה
        </button>
      </form>


      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {testimonials.map((t, i) => (
          <blockquote
            key={i}
            className="p-4 border-r-4 border-blue-500 bg-blue-50 rounded shadow-sm"
          >
            <p className="mb-2">"{t.text}"</p>
            <footer className="flex justify-between text-sm text-gray-700">
              <span>{t.name}</span>
              <span>{renderStars(t.rating)}</span>
            </footer>
          </blockquote>
        ))}
      </div>
    </section>
  );
};

export default TestimonialsSection;
