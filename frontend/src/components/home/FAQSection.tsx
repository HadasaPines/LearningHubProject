

const questions = [
  { q: "איך נרשמים?", a: "דרך עמוד ההרשמה או התחברות." },
  { q: "האם אפשר לבטל שיעור?", a: "כן, בתיאום מול המורה." },
];

const FAQSection = () => (
  <section className="p-10 bg-gray-50">
    <h2 className="text-2xl font-bold mb-4">שאלות נפוצות</h2>
    <ul>
      {questions.map((qa, i) => (
        <li key={i} className="mb-2">
          <strong>{qa.q}</strong>
          <p>{qa.a}</p>
        </li>
      ))}
    </ul>
  </section>
);

export default FAQSection;