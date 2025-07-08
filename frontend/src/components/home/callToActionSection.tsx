
import { useNavigate } from "react-router-dom";

const CallToActionSection = () => {
  const navigate = useNavigate();

  return (
    <section className="p-10 bg-blue-600 text-white text-center">
      <h2 className="text-3xl font-bold mb-4">הצטרפו אלינו עוד היום!</h2>
      <p className="mb-6 text-lg">חווית לימוד אישית, גישה למורים מעולים ושיעורים באיכות גבוהה</p>

      <div className="flex justify-center gap-4 flex-wrap">
        <button
          className="bg-white text-blue-600 font-bold py-2 px-6 rounded hover:bg-blue-100"
          onClick={() => navigate("/register")}
        >
          הרשמה
        </button>

        <button
          className="bg-white text-blue-600 font-bold py-2 px-6 rounded hover:bg-blue-100"
          onClick={() => navigate("/login")}
        >
          התחברות
        </button>
      </div>

      <p className="mt-6 text-sm text-white">
        מורה? נשמח לצרף אותך למערך ההוראה שלנו –{" "}
        <span className="underline cursor-pointer hover:text-yellow-300" onClick={() => navigate("/contact")}>
          צרו קשר עם מנהל המערכת
        </span>
      </p>
    </section>
  );
};

export default CallToActionSection;
