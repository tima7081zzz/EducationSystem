import React, { useEffect, useState } from 'react';
import {
  Container,
  Typography,
  Grid,
  Fab,
  AppBar,
  Tabs,
  Tab,
  Box,
  List,
  ListItem,
  ListItemText,
  Paper,
  CircularProgress,
  IconButton
} from '@mui/material';
import { useParams, Link } from 'react-router-dom';
import CourseHeader from '../components/course/CourseHeader';
import CoursePublicId from '../components/course/CoursePublicId';
import AssignmentList from '../components/course/AssignmentList';
import AddAssignmentModal from '../components/course/AddAssignmentModal';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import { getCourseDetails, CourseModel, getCourseUsers, CourseUsersModel, deleteStudentFromCourse } from '../services/courseService';
import { addAssignment, AddAssignmentRequestModel } from '../services/assignmentService';

const colors = [
  '#BBDEFB', // light blue 
];

const CourseDetailsPage: React.FC = () => {
  const { courseId } = useParams<{ courseId: string }>();
  const [headerColor, setHeaderColor] = useState<string>('');
  const [course, setCourse] = useState<CourseModel | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [open, setOpen] = useState<boolean>(false);
  const [currentTab, setCurrentTab] = useState<number>(0);
  const [courseUsers, setCourseUsers] = useState<CourseUsersModel | null>(null);
  const [isUsersLoading, setIsUsersLoading] = useState<boolean>(false);
  const [usersError, setUsersError] = useState<string | null>(null);

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

  const handleChangeTab = (event: React.SyntheticEvent, newValue: number) => {
    setCurrentTab(newValue);
  };

  useEffect(() => {
    if (currentTab === 1 && !courseUsers) {
      const fetchCourseUsers = async () => {
        setIsUsersLoading(true);
        setUsersError(null);
        try {
          if (courseId) {
            const users = await getCourseUsers(Number(courseId));
            setCourseUsers(users);
          }
        } catch (err) {
          setUsersError('Failed to fetch course users.');
        } finally {
          setIsUsersLoading(false);
        }
      };

      fetchCourseUsers();
    }
  }, [currentTab, courseId, courseUsers]);

  const handleDeleteStudent = async (studentId: number) => {
    if (courseId) {
      try {
        await deleteStudentFromCourse(studentId);
        const users = await getCourseUsers(Number(courseId));
        setCourseUsers(users);
      } catch (err) {
        alert('Failed to delete student from course.');
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
      <AppBar position="static" color="default" elevation={0}>
        <Tabs value={currentTab} onChange={handleChangeTab} indicatorColor="primary" textColor="primary">
          <Tab label="Feed" />
          <Tab label="Users" />
        </Tabs>
      </AppBar>
      <Box hidden={currentTab !== 0}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={3}>
            {course.isTeacher && (
              <CoursePublicId publicId={course.publicId} handleCopyPublicId={handleCopyPublicId} />
            )}
          </Grid>
          <Grid item xs={12} md={9}>
            <AssignmentList
              courseId={course.id.toString()}
              assignments={course.assignments.map((a) => ({
                ...a,
                creationDate: new Date(a.createdAt).toLocaleDateString(),
              }))}
              isTeacher={course.isTeacher}
            />
          </Grid>
        </Grid>
      </Box>
      <Box hidden={currentTab !== 1}>
        {isUsersLoading ? (
          <CircularProgress />
        ) : usersError ? (
          <Typography color="error">{usersError}</Typography>
        ) : courseUsers ? (
          <Box>
            <Typography variant="h5" gutterBottom>Teachers</Typography>
            <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
              <List>
                {courseUsers.teachers.map((teacher) => (
                  <ListItem key={teacher.courseUserId}>
                    <ListItemText primary={teacher.fullname} />
                  </ListItem>
                ))}
              </List>
            </Paper>
            <Typography variant="h5" gutterBottom>Students</Typography>
            <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
              <List>
                {courseUsers.students.map((student) => (
                  <ListItem
                    key={student.courseUserId}
                    secondaryAction={
                      course.isTeacher ? (
                        <IconButton edge="end" aria-label="delete" onClick={() => handleDeleteStudent(student.courseUserId)}>
                          <DeleteIcon />
                        </IconButton>
                      ) : null
                    }
                  >
                    <ListItemText primary={student.fullname} />
                  </ListItem>
                ))}
              </List>
            </Paper>
          </Box>
        ) : null}
      </Box>
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
