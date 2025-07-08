

const HomeSection = () => (
  <section className="text-center p-10 bg-blue-100">
    <h1 className="text-4xl font-bold mb-4">ברוכים הבאים ל־Learning Hub</h1>
    <p className="text-xl mb-6">לימוד מותאם אישית לכל תלמיד</p>
    <div className="flex justify-center gap-4">
      <button  className="bg-blue-500 text-white py-2 px-4 rounded">התחברות</button>
      <button className="bg-green-500 text-white py-2 px-4 rounded">הצטרפות כתלמיד</button>
      <button className="bg-purple-500 text-white py-2 px-4 rounded">הצטרפות כמורה</button>
    </div>
  </section>
);

export default HomeSection;