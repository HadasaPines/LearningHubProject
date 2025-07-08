import { useEffect, useState } from "react";
import { getAllTeachers } from "../../services/api";
import type { User } from "../../models/userModel";

const OurTeachersSection = () => {
  const [teachers, setTeachers] = useState<User[]>([]);

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const res = await getAllTeachers();
        setTeachers(res.data);
      } catch (error) {
        console.error("שגיאה בטעינת מורים:", error);
      }
    };
    fetchTeachers();
  }, []);

  return (
    <section className="py-12 px-6 bg-gradient-to-b from-white to-blue-50">
      <h2 className="text-3xl font-bold text-center text-blue-800 mb-10">
        הכירו את צוות המורים שלנו
      </h2>

      <div className="grid gap-8 md:grid-cols-3 sm:grid-cols-1">
        {teachers.map((teacher) => (
          <div
            key={teacher.userId}
            className="bg-white shadow-xl rounded-2xl p-6 text-center hover:scale-105 transition-transform duration-300"
          >
           <div className="w-24 h-24 mx-auto bg-blue-100 text-blue-600 rounded-full flex items-center justify-center text-3xl">
  👤
</div>
            <h3 className="text-xl font-semibold text-blue-700">
              {teacher.firstName} {teacher.lastName}
            </h3>
            <p className="text-gray-600 text-sm mt-1">{teacher.email}</p>

            {teacher.teacher?.bio && (
              <p className="mt-4 text-gray-700 text-sm italic leading-relaxed">
                {teacher.teacher.bio}
              </p>
            )}

          
          </div>
        ))}
      </div>
    </section>
  );
};

export default OurTeachersSection;
