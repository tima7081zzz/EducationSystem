export interface Course {
    id: number;
    title: string;
    description: string;
  }
  
  export const fetchCourses = async (): Promise<Course[]> => {
    const response = await fetch('http://localhost:8181/courses');
    if (!response.ok) {
      throw new Error('Failed to fetch courses');
    }
    return response.json();
  };
  