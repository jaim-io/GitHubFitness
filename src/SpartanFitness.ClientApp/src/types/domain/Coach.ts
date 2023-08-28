type Coach = {
  id: string;
  userId: string;
  firstName: string;
  lastName: string;
  biography: string;
  socialMedia: SocialMedia;
  profileImage: string;
  email: string;
  createdDateTime: Date;
  updatedDateTime: Date;
};

type SocialMedia = {
  linkedInUrl?: string;
  websiteUrl?: string;
  instagramUrl?: string;
  facebookUrl?: string;
};

export default Coach;
