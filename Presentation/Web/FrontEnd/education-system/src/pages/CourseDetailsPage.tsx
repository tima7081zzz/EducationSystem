import React, { useEffect, useState } from 'react';
import { Container, Typography, Grid, Paper, Box, Button, List, ListItem, ListItemText, Divider, Fab, Modal, TextField, MenuItem } from '@mui/material';
import { useParams } from 'react-router-dom';
import { getCourseDetails, CourseModel } from '../services/courseService';
import { addAssignment } from '../services/assignmentService';
import AddIcon from '@mui/icons-material/Add';

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
  const [newAssignment, setNewAssignment] = useState({
    title: '',
    description: '',
    maxGrade: '',
    deadline: ''
  });

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

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewAssignment((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (courseId && course) {
      try {
        await addAssignment({
          courseId: Number(courseId),
          title: newAssignment.title,
          description: newAssignment.description,
          maxGrade: newAssignment.maxGrade ? Number(newAssignment.maxGrade) : undefined,
          deadline: new Date(newAssignment.deadline).toISOString(),
          userId: 1 // Replace with actual userId
        });
        setOpen(false);
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
      <Box 
        sx={{ 
          backgroundColor: headerColor, 
          padding: '16px', 
          borderRadius: '8px', 
          mb: 3,
          textAlign: 'center'
        }}
      >
        <Typography variant="h4" component="h1" gutterBottom>
          {course.name}
        </Typography>
        <Typography variant="body1" component="p">
          {course.description}
        </Typography>
      </Box>

      <Grid container spacing={3}>
        <Grid item xs={12} md={3}>
          {course.isTeacher && (
            <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
              <Typography variant="subtitle1" gutterBottom>
                Course Public ID
              </Typography>
              <Typography variant="body2" sx={{ mb: 2 }}>
                {course.publicId}
              </Typography>
              <Button variant="outlined" onClick={handleCopyPublicId}>
                Copy it
              </Button>
            </Paper>
          )}
        </Grid>
        <Grid item xs={12} md={9}>
          <Paper elevation={3} sx={{ padding: '16px' }}>
            <Typography variant="h6" gutterBottom>
              Assignments
            </Typography>
            <List>
              {course.assignments.map((assignment) => (
                <div key={assignment.id}>
                  <ListItem button component="a" href={`/course/${course.id}/assignment/${assignment.id}`}>
                    <ListItemText primary={assignment.title} secondary={`Created on: ${new Date(assignment.createdAt).toLocaleDateString()}`} />
                  </ListItem>
                  <Divider />
                </div>
              ))}
            </List>
          </Paper>
        </Grid>
      </Grid>

      {course.isTeacher && (
        <>
          <Fab color="primary" aria-label="add" onClick={handleOpen} sx={{ position: 'fixed', bottom: 16, right: 16 }}>
            <AddIcon />
          </Fab>
          <Modal open={open} onClose={handleClose}>
            <Paper sx={{ padding: '16px', margin: 'auto', maxWidth: '500px', mt: '10%' }}>
              <Typography variant="h6" gutterBottom>
                Add Assignment
              </Typography>
              <form onSubmit={handleSubmit}>
                <TextField
                  fullWidth
                  label="Title"
                  name="title"
                  value={newAssignment.title}
                  onChange={handleInputChange}
                  required
                  sx={{ mb: 2 }}
                />
                <TextField
                  fullWidth
                  label="Description"
                  name="description"
                  value={newAssignment.description}
                  onChange={handleInputChange}
                  multiline
                  rows={4}
                  sx={{ mb: 2 }}
                />
                <TextField
                  fullWidth
                  label="Max Grade"
                  name="maxGrade"
                  value={newAssignment.maxGrade}
                  onChange={handleInputChange}
                  type="number"
                  sx={{ mb: 2 }}
                />
                <TextField
                  fullWidth
                  label="Deadline"
                  name="deadline"
                  value={newAssignment.deadline}
                  onChange={handleInputChange}
                  type="datetime-local"
                  sx={{ mb: 2 }}
                />
                <Button type="submit" variant="contained" color="primary">
                  Add Assignment
                </Button>
              </form>
            </Paper>
          </Modal>
        </>
      )}
    </Container>
  );
};

export default CourseDetailsPage;
