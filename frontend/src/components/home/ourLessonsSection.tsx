import { useEffect, useState } from "react";
import { getAllLessons, getAllSubjects } from "../../services/api";
import type { Lesson } from "../../models/lessonModel";
import type { Subject } from "../../models/subjectModel";

const OurLessonsSection = () => {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const [lessonsRes, subjectsRes] = await Promise.all([
        getAllLessons(),
        getAllSubjects(),
      ]);
      setLessons(lessonsRes.data);
      setSubjects(subjectsRes.data);
    };
    fetchData();
  }, []);

  const lessonsBySubject = subjects.map((subject) => {
    const subjectLessons = lessons.filter(
      (l) => l.subjectId === subject.subjectId
    );
    return {
      ...subject,
      lessons: subjectLessons,
    };
  });

  return (
    <section className="p-10 bg-gray-50">
      <h2 className="text-2xl font-bold mb-6 text-center">מה תוכלו ללמוד אצלנו?</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {lessonsBySubject.map((s) => (
          <div
            key={s.subjectId}
            className="bg-white p-5 rounded shadow border flex flex-col justify-between"
          >
            <div>
              <h3 className="text-xl font-semibold mb-2">{s.name}</h3>
              {s.lessons.length > 0 ? (
                <ul className="text-sm text-gray-600">
                  {s.lessons.slice(0, 3).map((l) => (
                    <li key={l.lessonId}>
                      📅 {l.lessonDate} | 🕒 {l.startTime}-{l.endTime}
                    </li>
                  ))}
                  {s.lessons.length > 3 && (
                    <li className="text-blue-600 mt-1">ועוד {s.lessons.length - 3} שיעורים...</li>
                  )}
                </ul>
              ) : (
                <p className="text-sm text-gray-400">אין כרגע שיעורים זמינים בנושא זה.</p>
              )}
            </div>
            {s.lessons.length > 0 && (
              <button className="mt-4 bg-blue-500 text-white py-1 px-4 rounded hover:bg-blue-600">
                הרשמה
              </button>
            )}
          </div>
        ))}
      </div>
    </section>
  );
};

export default OurLessonsSection;
