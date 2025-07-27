import { useEffect, useState, type JSX } from 'react';
import styles from './ourSubjectsSection.module.scss';
import {
  BookOpen,
  Globe,
  FlaskConical,
  Microscope,
  Landmark,
  SpellCheck,
  Languages,
  Sigma
} from 'lucide-react';
import { getAllSubjects } from '../../services/api';

interface Subject {
  id: string;
  name: string;
  description: string;
}

const iconMap: Record<string, JSX.Element> = {
  Literature: <BookOpen size={40} />,
  Geography: <Globe size={40} />,
  Sciences: <FlaskConical size={40} />,
  Biology: <Microscope size={40} />,
  History: <Landmark size={40} />,
  Grammar: <SpellCheck size={40} />,
  English: <Languages size={40} />,
  Math: <Sigma size={40} />
};

const OurSubjectsSection = () => {
  const [subjects, setSubjects] = useState<Subject[]>([]);

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const res = await getAllSubjects();
        setSubjects(res.data);
      } catch (error) {
        console.error('Failed to load subjects:', error);
      }
    };
    fetchSubjects();
  }, []);

  return (
    <div className={styles.pageWrapper}>
      <div className={styles.verticalTitle}>Our world of knowledge</div>
      <div className={styles.subjectsGrid}>
        {subjects.map((subject) => {
          const icon = iconMap[subject.name] || <BookOpen size={40} />;
          return (
            <div key={subject.id} className={styles.subjectCard}>
              <div className={styles.iconCircle}>{icon}</div>
              <div className={styles.subjectName}>{subject.name}</div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
export default OurSubjectsSection;
