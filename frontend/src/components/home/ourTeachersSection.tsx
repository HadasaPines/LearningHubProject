import { useEffect, useState } from "react";
import { getAllTeachers } from "../../services/api";
import type { User } from "../../models/userModel";
import styles from './OurTeachersSection.module.scss';
import { FiUser } from "react-icons/fi";

const OurTeachersSection = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [expanded, setExpanded] = useState<{ [key: string]: boolean }>({});

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const res = await getAllTeachers();
        setTeachers(res.data);
      } catch (error) {
        console.error("Failed to load teachers:", error);
      }
    };
    fetchTeachers();
  }, []);

  const toggleReadMore = (id: number) => {
    setExpanded(prev => ({ ...prev, [id]: !prev[id] }));
  };

  const splitToLines = (text: string, lineLength: number): string[] => {
    const lines = [];
    for (let i = 0; i < text.length; i += lineLength) {
      lines.push(text.slice(i, i + lineLength));
    }
    return lines;
  };

  return (
    <section className={styles.ourTeachersSection}>
      <h2 className={styles.title}>Meet Our Teaching Team</h2>

      <div className={styles.teachersWrapper}>
        <div className={styles.teachersGrid}>
          {teachers.map((teacher) => {
            const fullName = `${teacher.firstName} ${teacher.lastName}`;
            const bio = teacher.teacher?.bio || "";
            const isLong = bio.length > 13;
            const isExpanded = expanded[teacher.userId] || false;

            const displayedBio = isExpanded
              ? splitToLines(bio, 13)
              : [bio.slice(0, 13)];

            return (
              <div key={teacher.userId} className={styles.teacherItem}>
                <div className={styles.userIcon}><FiUser /></div>
                <h3 className={styles.name}>{fullName}</h3>
                {bio && (
                  <div className={styles.bio}>
                    {displayedBio.map((line, index) => (
                      <p key={index} className={styles.bioLine}>{line}</p>
                    ))}
                    {isLong && (
                      <span
                        className={styles.readMore}
                        onClick={() => toggleReadMore(teacher.userId)}
                      >
                        {isExpanded ? "Show Less" : "Read More"}
                      </span>
                    )}
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </section>
  );
};

export default OurTeachersSection;
