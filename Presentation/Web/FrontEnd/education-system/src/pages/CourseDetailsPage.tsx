import React, { useEffect, useState } from 'react';
import { Container, Typography, Grid, Paper, Box, Button, List, ListItem, ListItemText, Divider } from '@mui/material';
import { useParams } from 'react-router-dom';

interface Assignment {
  id: number;
  title: string;
  creationDate: string;
}

const colors = [
  '#FFCDD2', // light red
  '#C8E6C9', // light green
  '#BBDEFB', // light blue
  '#FFE0B2', // light orange
  '#E1BEE7', // light purple
];

const CourseDetailsPage: React.FC = () => {
  const { courseId } = useParams<{ courseId: string }>();
  const isTeacher = true; // This should be derived from your actual data
  const [headerColor, setHeaderColor] = useState<string>('');

  useEffect(() => {
    const randomColor = colors[Math.floor(Math.random() * colors.length)];
    setHeaderColor(randomColor);
  }, []);

  const course = {
    id: courseId,
    name: 'Sample Course',
    description: 'This is a sample course description.',
    publicId: '12345-ABCDE',
    assignments: [
      { id: 1, title: 'Assignment 1', creationDate: '2024-01-01' },
      { id: 2, title: 'Assignment 2', creationDate: '2024-02-01' },
    ],
  };

  const handleCopyPublicId = () => {
    navigator.clipboard.writeText(course.publicId);
    alert('Course ID copied to clipboard');
  };

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
          {isTeacher && (
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
                    <ListItemText primary={assignment.title} secondary={`Created on: ${assignment.creationDate}`} />
                  </ListItem>
                  <Divider />
                </div>
              ))}
            </List>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default CourseDetailsPage;
