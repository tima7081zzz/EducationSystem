import React from 'react';
import { Grid } from '@mui/material';
import CourseCard from './CourseCard';

const courses = [
  { id: 1, title: 'Course 1', description: 'Description of course 1' },
  { id: 2, title: 'Course 2', description: 'Description of course 2' },
  // Add more courses as needed
];

const CourseGrid: React.FC = () => {
  return (
    <Grid container spacing={3}>
      {courses.map(course => (
        <Grid item xs={12} sm={6} md={4} key={course.id}>
          <CourseCard id={course.id} title={course.title} description={course.description} />
        </Grid>
      ))}
    </Grid>
  );
};

export default CourseGrid;
