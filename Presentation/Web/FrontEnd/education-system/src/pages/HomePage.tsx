import React, { useState } from 'react';
import {
  Container, Typography, Fab, Menu, MenuItem, Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Grid, Card, CardContent, CardActions
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { Link } from 'react-router-dom';

// Mock data for courses
const courses = [
  { id: 1, title: 'Course 1', description: 'Description of course 1' },
  { id: 2, title: 'Course 2', description: 'Description of course 2' },
  // Add more courses as needed
];

const HomePage: React.FC = () => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [joinCourseOpen, setJoinCourseOpen] = useState(false);
  const [createCourseOpen, setCreateCourseOpen] = useState(false);
  const [courseId, setCourseId] = useState('');
  const [courseName, setCourseName] = useState('');
  const [courseDescription, setCourseDescription] = useState('');
  const [courseCategory, setCourseCategory] = useState('');
  const [createCourseError, setCreateCourseError] = useState('');

  const handleFabClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleFabClose = () => {
    setAnchorEl(null);
  };

  const handleJoinCourseOpen = () => {
    setJoinCourseOpen(true);
    handleFabClose();
  };

  const handleCreateCourseOpen = () => {
    setCreateCourseOpen(true);
    handleFabClose();
  };

  const handleJoinCourseClose = () => {
    setJoinCourseOpen(false);
  };

  const handleCreateCourseClose = () => {
    setCreateCourseOpen(false);
    setCreateCourseError('');
  };

  const handleJoinCourse = () => {
    // Handle join course logic here
    console.log('Joining course with ID:', courseId);
    handleJoinCourseClose();
  };

  const handleCreateCourse = () => {
    if (!courseName) {
      setCreateCourseError('Course Name is required');
      return;
    }
    // Handle create course logic here
    console.log('Creating course with Name:', courseName, 'Description:', courseDescription, 'Category:', courseCategory);
    handleCreateCourseClose();
  };

  return (
    <Container>
      <Typography variant="h4" component="h1" gutterBottom>
        My Courses
      </Typography>
      <Grid container spacing={4}>
        {courses.map((course) => (
          <Grid item key={course.id} xs={12} sm={6} md={4}>
            <Card>
              <CardContent>
                <Typography variant="h5" component="h2">
                  {course.title}
                </Typography>
                <Typography variant="body2" component="p">
                  {course.description}
                </Typography>
              </CardContent>
              <CardActions>
                <Button component={Link} to={`/course/${course.id}`} size="small">
                  Open
                </Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Fab color="primary" aria-label="add" onClick={handleFabClick} style={{ position: 'fixed', bottom: 16, right: 16 }}>
        <AddIcon />
      </Fab>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleFabClose}
      >
        <MenuItem onClick={handleJoinCourseOpen}>Join course</MenuItem>
        <MenuItem onClick={handleCreateCourseOpen}>Create course</MenuItem>
      </Menu>

      {/* Join Course Dialog */}
      <Dialog open={joinCourseOpen} onClose={handleJoinCourseClose}>
        <DialogTitle>Join Course</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Course ID"
            type="text"
            fullWidth
            value={courseId}
            onChange={(e) => setCourseId(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleJoinCourseClose} color="primary">
            Cancel
          </Button>
          <Button onClick={handleJoinCourse} color="primary">
            Join
          </Button>
        </DialogActions>
      </Dialog>

      {/* Create Course Dialog */}
      <Dialog open={createCourseOpen} onClose={handleCreateCourseClose}>
        <DialogTitle>Create Course</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Course Name"
            type="text"
            fullWidth
            required
            value={courseName}
            onChange={(e) => setCourseName(e.target.value)}
            error={!!createCourseError}
            helperText={createCourseError}
          />
          <TextField
            margin="dense"
            label="Description"
            type="text"
            fullWidth
            value={courseDescription}
            onChange={(e) => setCourseDescription(e.target.value)}
          />
          <TextField
            margin="dense"
            label="Category"
            type="text"
            fullWidth
            value={courseCategory}
            onChange={(e) => setCourseCategory(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCreateCourseClose} color="primary">
            Cancel
          </Button>
          <Button onClick={handleCreateCourse} color="primary">
            Create
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default HomePage;
