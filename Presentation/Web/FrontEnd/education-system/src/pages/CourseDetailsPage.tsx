import React, { useEffect, useState } from 'react';
import { Container, Typography, Grid, Fab } from '@mui/material';
import { useParams } from 'react-router-dom';
import CourseHeader from '../components/course/CourseHeader';
import CoursePublicId from '../components/course/CoursePublicId';
import AssignmentList from '../components/course/AssignmentList';
import AddAssignmentModal from '../components/course/AddAssignmentModal';
import AddIcon from '@mui/icons-material/Add';
import { getCourseDetails, CourseModel } from '../services/courseService';
import { addAssignment, AddAssignmentRequestModel } from '../services/assignmentService';

const colors = [
  '#FFCDD2', // light red
  '#C8E6C9', // light green
  '#BBDEFB', // light blue
  '#FFE0B2', // light orange
  '#E1BEE7', // light purple
];

const CourseDetailsPage: React.FC = () => {
  const { courseId } = useParams<{ courseId: string }>();
  const [headerColor, setHeaderColor] = useState<string>('');
  const [course, setCourse] = useState<CourseModel | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [open, setOpen] = useState<boolean>(false);

  useEffect(() => {
    const randomColor = colors[Math.floor(Math.random() * colors.length)];
    setHeaderColor(randomColor);

    const fetchCourseDetails = async () => {
      try {
        if (courseId) {
          const data = await getCourseDetails(Number(courseId));
          setCourse(data);
        }
      } catch (err) {
        setError('Failed to fetch course details.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchCourseDetails();
  }, [courseId]);

  const handleCopyPublicId = () => {
    if (course?.publicId) {
      navigator.clipboard.writeText(course.publicId);
      alert('Course ID copied to clipboard');
    }
  };

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleAddAssignment = async (assignment: AddAssignmentRequestModel) => {
    if (courseId) {
      try {
        await addAssignment(assignment);
        const data = await getCourseDetails(Number(courseId));
        setCourse(data);
      } catch (err) {
        alert('Failed to add assignment.');
      }
    }
  };

  if (isLoading) {
    return <Typography>Loading...</Typography>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!course) {
    return <Typography>Course not found.</Typography>;
  }

  return (
    <Container>
      <CourseHeader name={course.name} description={course.description} headerColor={headerColor} />
      <Grid container spacing={3}>
        <Grid item xs={12} md={3}>
          {course.isTeacher && (
            <CoursePublicId publicId={course.publicId} handleCopyPublicId={handleCopyPublicId} />
          )}
        </Grid>
        <Grid item xs={12} md={9}>
          <AssignmentList courseId={course.id.toString()} assignments={course.assignments.map((a) => ({
            ...a,
            creationDate: new Date(a.createdAt).toLocaleDateString(),
          }))} />
        </Grid>
      </Grid>
      {course.isTeacher && (
        <>
          <Fab color="primary" aria-label="add" onClick={handleOpen} sx={{ position: 'fixed', bottom: 16, right: 16 }}>
            <AddIcon />
          </Fab>
          <AddAssignmentModal open={open} handleClose={handleClose} handleAddAssignment={handleAddAssignment} courseId={course.id} />
        </>
      )}
    </Container>
  );
};

export default CourseDetailsPage;
